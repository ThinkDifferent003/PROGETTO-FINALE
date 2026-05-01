using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State {IDLE,CHASING,CIRCLING,ATTACKING,RETREATING}
    [Header("Current State")]
    public State _currentState;
    private bool _isRecoiling = false;

    [Header("Detection & Combat Ranges")]
    [SerializeField] private float _circleRange = 5f; //Distanza a cui il nemico inizia a girare intorno al player
    [SerializeField] private float _attackRange = 1.5f; //Distanza minima per far partire l'attacco
    [SerializeField] private float _detectionRange = 1f; //Raggio di vista per attivarsi

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f; //Velocità movimento
    [SerializeField] private float _attackCooldown = 0.9f; //Tempo di attesa tra gli attacchi

    [Header("Behavior Settings")]
    [SerializeField] private float _aggressionTime = 1f; //Probabilità di attaccare quando è vicino
    [SerializeField] private float _patienceTime = 0.3f; //probabilità di mettersi in IDLE mentre circonda

    [Header("Recoil Settings")]
    [SerializeField] private float _forceRecoil = 0.5f; //Forza della spinta
    [SerializeField] private float _durationRecoil = 0.2f; //Durata del blocco
    private float _timerRecoil = 0f; //Contatore timer del rinculo

    
    private float _decisionTimer; //Timer per bloccare AI in uno stato per un certo tempo
    private float _attackTimer; //Timer per il cooldown dell attacco

    private Transform _player;
    private EnemyAnimation _enemyAnimation;
    private NavMeshAgent _agent;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        _agent = GetComponent<NavMeshAgent>();
        if ( _agent != null )
        {
            _agent.speed = _moveSpeed;
            _agent.updateRotation = false;
        }
        _currentState = State.IDLE;
    }
    private void Update()
    {
        if (_player ==  null || _isRecoiling) return;
        //Calcola la distanza tra :      (    Nemico        )&(   Player      )
        float distance = Vector3.Distance(transform.position, _player.position);
        StateTransitions(distance); //Gestice gli stati
        ExecuteCurrentState();
        _decisionTimer -= Time.deltaTime; //Riduce il timer
        UpdateAnimation(); //Aggiorna le animazioni
    }
    #region State Machine Logic
    private void StateTransitions(float distance)
    {
        if (_currentState == State.ATTACKING) //Se sta attaccando
        {
            if (_decisionTimer <=  0) ChangeState(State.RETREATING, 1.0f); //Si ritira     
            return;
        }

        if (_currentState == State.IDLE) //Se sta in Idle
        {
            if (distance <= _detectionRange) ChangeState(State.CHASING, 0.5f); //Insegue    
            return;
        }

        if (distance >  _detectionRange * 1.5f) //Se la distanza è maggiore della vista del nemico
        {
            ChangeState(State.IDLE, 1.0f); //Va in Idle
            return;
        }
        
        if (_decisionTimer > 0) return; //Non cambia stato se il timer non è a 0

        if (distance > _circleRange) //Se la distanza è maggiore
        {
            ChangeState(State.CHASING, 0.5f); //Va in inseguimento
        } 
        else if (distance <= _attackRange) //Se la distanza è minore-uguale al range minimo di attacco
        {
            CombatDecision();
        }
        else
        {
            CircleDecision();
        }
    }
    private void CombatDecision()
    {
        if (Time.time < _attackTimer) return;//Controllo se è passatto abbastanza tempo dall'ulitmo attacco
       
        if (Random.value < _aggressionTime) //Vedo a caso , se vuole attaccare
        {
             ChangeState(State.ATTACKING, 0.8f); //Attacca
             if (_enemyAnimation != null) _enemyAnimation.AnimationAttack(); //Parte l'animazione 
            _attackTimer = Time.time + _attackCooldown; //Aggionra il timer
        }
        else //Se non vuole attaccare 
        {
            ChangeState(State.RETREATING, Random.Range(0.5f, 1f)); //Si ritira
        }
    }
    private void CircleDecision()
    {
        if (Random.value < _patienceTime) ChangeState(State.IDLE, Random.Range(0.5f, 1.5f)); 
        else ChangeState(State.CIRCLING, Random.Range(1f, 3f));
    }
    private void ChangeState(State newState , float duration)
    {
        if (_currentState == newState && newState != State.ATTACKING) return;
        _currentState = newState;
        _decisionTimer = duration;
        Debug.Log("Stato cambiato in: " + newState);
    }
    #endregion
    #region Execution Methods
    private void ExecuteCurrentState()
    {
        switch (_currentState)
        {
            case State.IDLE: StopMovement(); break;

            case State.CHASING: SetAgentDestination(_player.position); break;

            case State.CIRCLING: Circling(); break;

            case State.ATTACKING: StopMovement(); break;

            case State.RETREATING: Retreating(); break;
        }
    }
    private void StopMovement()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }
    private void SetAgentDestination(Vector3 terget)
    {
        if (!_agent.enabled) return;
        _agent.isStopped = false;
        _agent.SetDestination(terget);
    }
    private void Circling()
    {
        //Calcola la direzione dal player al nemico
        Vector3 directionToPlayer = (transform.position - _player.position).normalized;
        //Calcola la direzione perpendicolare
        Vector3 sideDirection = Vector3.Cross(directionToPlayer, Vector3.up);
        //Punto di destinazione
        Vector3 targetPoint = _player.position + (directionToPlayer * _circleRange) + sideDirection;
        SetAgentDestination(targetPoint);
    }
    private void Retreating()
    {
        Vector3 directionToPlayer = (transform.position - _player.position).normalized;
        Vector3 retreatPosition = transform.position + directionToPlayer * 3f;
        SetAgentDestination(retreatPosition);
    }
    #endregion
    #region Feedbacks & Animations
    public IEnumerator Recoil(Vector3 direction)
    {
        if (_agent == null || !_agent.enabled) yield break;
        _isRecoiling = true; //Attivo il rinculo
        _agent.isStopped = true; //Blocco l'agent
        _timerRecoil = 0f;
        //Ciclo che sposta il nemico finche il timer non raggiunge la fine
        while (_timerRecoil < _durationRecoil)
        {
            transform.position += direction * _forceRecoil * Time.deltaTime; //Sposto il nemico
            _timerRecoil += Time.deltaTime;
            yield return null;
        }
        _agent.Warp(transform.position); //Comunica al agent che il nemico si è spostato
        _agent.isStopped = false; //Ri attivo l'agent  
        _isRecoiling = false;
    }
    private void UpdateAnimation()
    {
        if (_enemyAnimation == null) return;
        Vector3 direction;
        if (_currentState == State.ATTACKING || _currentState == State.IDLE) direction = (_player.position - transform.position).normalized;  
        else direction = _agent.velocity.normalized;
        bool isMovingNow = _agent.velocity.sqrMagnitude > 0.1f;
        _enemyAnimation.SetMoves(direction.x, direction.z, isMovingNow);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _circleRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
    #endregion
}

