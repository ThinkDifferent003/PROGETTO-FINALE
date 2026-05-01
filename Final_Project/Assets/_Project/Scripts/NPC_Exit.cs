using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Exit : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private float _stoppingDistance = 0.5f;
    private EnemyAnimation _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<EnemyAnimation>();
    }
    public void StartLeaving()
    {
        if (_agent != null)
        {
            _agent.enabled = true;
            _agent.isStopped = false;
        }
        StartCoroutine(LeaveRoutine());
    }
    private IEnumerator LeaveRoutine()
    {
        if (_agent == null || _exitPoint == null) yield break;
        _agent.enabled = true;
        _agent.isStopped = false;
        _agent.SetDestination(_exitPoint.position);
        if (_anim != null) _anim.SetMoves(0, 1, true);
        yield return new WaitUntil(() => !_agent.pathPending);
        while (_agent.remainingDistance  > _stoppingDistance)
        {
            yield return null;
        }
        if (_anim != null) _anim.SetMoves(0,0,false);
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.enabled = true;
        Destroy(gameObject);
    }
}
