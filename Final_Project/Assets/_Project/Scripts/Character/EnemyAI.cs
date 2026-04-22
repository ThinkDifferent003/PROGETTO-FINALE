using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State {IDLE,CHASING,CIRCLING,ATTACKING,RETREATING}
    public State _currentState;

    [SerializeField] private float _circleRange = 5f;
    [SerializeField] private float _attackRange = 1.5f;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _attackCooldown = 0.9f;

    [SerializeField] private float _aggressionTime = 1f;
    [SerializeField] private float _patienceTime = 0.3f;

    [SerializeField] private float _detectionRange = 1f;

    [SerializeField] private float _forceRecoil = 0.5f;
    [SerializeField] private float _durationRecoil = 0.2f;
    [SerializeField] private float _timerRecoil = 0f;

    private bool _isRecoiling= false;
    private float _decisionTimer;
    private float _attackTimer;
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

        float distance = Vector3.Distance(transform.position, _player.position);

        StateTransitions(distance);

        switch (_currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.CHASING:
                Chasing();
                break;
            case State.CIRCLING:
                Circling();
                break;
            case State.ATTACKING:
                Attacking(); 
                break;
            case State.RETREATING:
                Retreating(); 
                break;
                
        }
        _decisionTimer -= Time.deltaTime;
        UpdateAnimation();
    }

    private void StateTransitions(float distance)
    {
        if (_currentState == State.ATTACKING)
        {
            if (_decisionTimer <=  0)
            {
                ChangeState(State.RETREATING, 1.0f);
            }
            return;
        }
        if (_currentState == State.IDLE)
        {
            if (distance <= _detectionRange)
            {
                ChangeState(State.CHASING, 0.5f);
            }
            return;
        }
        if (distance >  _detectionRange * 1.5f)
        {
            ChangeState(State.IDLE, 1.0f);
            return;
        }
        if (_decisionTimer > 0) return;

        if (_currentState == State.IDLE && distance <= _detectionRange)
        {
            ChangeState(State.CHASING, 0.5f);
        }

        if (_decisionTimer > 0) return;

        if (distance > _circleRange)
        {
            ChangeState(State.CHASING, 0.5f);
            return;
        }
        if (distance <= _attackRange)
        {
            if (Time.time >= _attackTimer)
            {
                if (Random.value < _aggressionTime)
                {
                    ChangeState(State.ATTACKING, 0.8f);
                    _enemyAnimation.AnimationAttack();
                    _attackTimer = Time.time + _attackCooldown;
                }
                else
                {
                    ChangeState(State.RETREATING, Random.Range(0.5f, 1f));
                }
                
            }
            return;
        }
        else if (distance <= _circleRange)
        {
            if (Random.value < _patienceTime)
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
        _isRecoiling = true;
        _agent.isStopped = true;
        while (_timerRecoil < _durationRecoil)
        {
            transform.position += direction * _forceRecoil * Time.deltaTime;
            _timerRecoil += Time.deltaTime;
            yield return null;
        }
        if (_agent != null && _agent.enabled)
        {
            _agent.Warp(transform.position);
            _agent.isStopped = false;
        }
        _isRecoiling = false;
    }
    private void Idle()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }
    private void Chasing()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
    }
    private void Circling()
    {
        _agent.isStopped = false;
        Vector3 directionToPlayer = (transform.position - _player.position).normalized;
        Vector3 sideDirection = Vector3.Cross(directionToPlayer, Vector3.up);
        Vector3 targetPoint = _player.position + (directionToPlayer * _circleRange) + sideDirection;
        _agent.SetDestination(targetPoint);
    }
    private void Attacking()
    {
        _agent.isStopped= true;
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
