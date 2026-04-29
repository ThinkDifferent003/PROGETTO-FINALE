using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State {IDLE,CHASING,CIRCLING,ATTACKING,RETREATING}
    public State _currentState;

    [SerializeField] private float _circleRange = 5f; //Distanza a cui il nemico inizia a girare intorno al player
    [SerializeField] private float _attackRange = 1.5f; //Distanza minima per far partire l'attacco

    [SerializeField] private float _moveSpeed = 3f; //Velocità movimento
    [SerializeField] private float _attackCooldown = 0.9f; //Tempo di attesa tra gli attacchi

    [SerializeField] private float _aggressionTime = 1f; //Probabilità di attaccare quando è vicino
    [SerializeField] private float _patienceTime = 0.3f; //probabilità di mettersi in IDLE mentre circonda

    [SerializeField] private float _detectionRange = 1f; //Raggio di vista per attivarsi

    [SerializeField] private float _forceRecoil = 0.5f; //Forza della spinta
    [SerializeField] private float _durationRecoil = 0.2f; //Durata del blocco
    [SerializeField] private float _timerRecoil = 0f; //Contatore timer del rinculo

    private bool _isRecoiling= false;
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

        _agent.speed = _moveSpeed;
        _agent.updateRotation = false;

        _currentState = State.IDLE;
    }
    private void Update()
    {
        if (_player ==  null) return;
        if (_isRecoiling) return;
        //Calcola la distanza tra :      (    Nemico        )&(   Player      )
        float distance = Vector3.Distance(transform.position, _player.position);

        StateTransitions(distance); //Gestice gli stati

        switch (_currentState)
        {
            case State.IDLE: Idle(); break;
            
            case State.CHASING: Chasing(); break;
            
            case State.CIRCLING: Circling(); break;
            
            case State.ATTACKING: Attacking(); break;
            
            case State.RETREATING: Retreating(); break;
        }
        _decisionTimer -= Time.deltaTime; //Riduce il timer
        UpdateAnimation(); //Aggiorna le animazioni
    }

    private void StateTransitions(float distance)
    {
        if (_currentState == State.ATTACKING) //Se sta attaccando
        {
            if (_decisionTimer <=  0) //Se il timer arriva a 0
            {
                ChangeState(State.RETREATING, 1.0f); //Si ritira
            }
            return;
        }
        if (_currentState == State.IDLE) //Se sta in Idle
        {
            if (distance <= _detectionRange) //Se la distanza è minore del raggio di vista del nemico
            {
                ChangeState(State.CHASING, 0.5f); //Insegue
            }
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
            return;
        }
        
        if (distance <= _attackRange) //Se la distanza è minore-uguale al range minimo di attacco
        {
            if (Time.time >= _attackTimer) //Controllo se è passatto abbastanza tempo dall'ulitmo attacco
            {
                if (Random.value < _aggressionTime) //Vedo a caso , se vuole attaccare
                {
                    ChangeState(State.ATTACKING, 0.8f); //Attacca
                    _enemyAnimation.AnimationAttack(); //Parte l'animazione 
                    _attackTimer = Time.time + _attackCooldown; //Aggionra il timer
                }
                else //Se non vuole attaccare 
                {
                    ChangeState(State.RETREATING, Random.Range(0.5f, 1f)); //Si ritira
                }
                
            }
            return;
        }
        else if (distance <= _circleRange) //Se invece la distanza e minore-uguale al raggio del cercio
        {
            if (Random.value < _patienceTime) //Decide a caso cosa fare
            {
                ChangeState(State.IDLE, Random.Range(0.5f, 1.5f));
            }
            else
            {
                ChangeState(State.CIRCLING, Random.Range(1f, 3f));
            }
        }
    }
    private void ChangeState(State newState , float duration)
    {
        if (_currentState == newState && newState != State.ATTACKING) return;
        _currentState = newState;
        _decisionTimer = duration;
        Debug.Log("Stato cambiato in: " + newState);
    }

    public IEnumerator Recoil(Vector3 direction)
    {
        if (_agent == null) yield break;
        _isRecoiling = true; //Attivo il rinculo
        _agent.isStopped = true; //Blocco l'agent
        //Ciclo che sposta il nemico finche il timer non raggiunge la fine
        while (_timerRecoil < _durationRecoil)
        {
            transform.position += direction * _forceRecoil * Time.deltaTime; //Sposto il nemico
            _timerRecoil += Time.deltaTime;
            yield return null;
        }
        if (_agent != null && _agent.enabled)
        {
            _agent.Warp(transform.position); //Comunica al agent che il nemico si è spostato
            _agent.isStopped = false; //Ri attivo l'agent
        }
        _isRecoiling = false;
    }
    private void Idle()
    {
        _agent.isStopped = true; //Fermno l'agent
        _agent.velocity = Vector3.zero; //Azzero la velocità
    }
    private void Chasing()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position); //Va alla posizione del player
    }
    private void Circling()
    {
        _agent.isStopped = false;
        //Calcola la direzione dal player al nemico
        Vector3 directionToPlayer = (transform.position - _player.position).normalized;
        //Calcola la direzione perpendicolare
        Vector3 sideDirection = Vector3.Cross(directionToPlayer, Vector3.up);
        //Punto di destinazione
        Vector3 targetPoint = _player.position + (directionToPlayer * _circleRange) + sideDirection;
        _agent.SetDestination(targetPoint);
    }
    private void Attacking()
    {
        _agent.isStopped= true; //Si ferma per attacare
        _agent.velocity = Vector3.zero;
    }
    private void Retreating()
    {
        _agent.isStopped= false;
        Vector3 directionToPlayer = (transform.position - _player.position).normalized;
        Vector3 retreatPosition = transform.position + directionToPlayer * 3f;
        _agent.SetDestination(retreatPosition);
    }
    private void UpdateAnimation()
    {
        if (_enemyAnimation == null) return;
        Vector3 direction;
        if (_currentState == State.ATTACKING || _currentState == State.IDLE)
        {
            direction = (_player.position - transform.position).normalized;
        }
        else
        {
            direction = _agent.velocity.normalized;
        }
        bool isMovingNow = _agent.velocity.magnitude > 0.1f;
        _enemyAnimation.SetMoves(direction.x, direction.z, isMovingNow);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _circleRange);
    }

}

