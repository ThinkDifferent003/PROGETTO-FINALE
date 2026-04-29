using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string _uiniqueID;
    [SerializeField] private TextAsset _inkJson;
    [SerializeField] private SO_CharacterStats _namePG;
    [SerializeField] private bool _onStart = false;
    [SerializeField] private bool _onlyNewGame = false;
    [SerializeField] private bool _isInteractable = false;
    [SerializeField] private bool _isOneShot = true;
    
    [SerializeField] private NavMeshAgent _enemyAgent;
    private bool _hasPlayed = false;
    private bool _playerInRange = false;

    private void Start()
    {
        if (_isOneShot && SaveManager.Instance != null && SaveManager.Instance._data != null)
        {
            if (SaveManager.Instance._data._collectedPickups.Contains(_uiniqueID))
            {
                Destroy(gameObject);
                return;
            }
        }
        if (_onStart)
        {
            if (_onlyNewGame && !MainMenuManager._intro)
            {
                return;
            }
            Execute();
            if (_onlyNewGame) MainMenuManager._intro = false;
        }
        
    }
    private void Update()
    {
        if (_isInteractable && _playerInRange && !_hasPlayed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Execute();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            if (!_isInteractable && !_onStart && !_hasPlayed)
            {
                Execute();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
    public void Execute()
    {
        if (_hasPlayed) return;
        _hasPlayed = true;
        if (_isOneShot && SaveManager.Instance != null && SaveManager.Instance._data != null)
        {
            if (!SaveManager.Instance._data._collectedPickups.Contains(_uiniqueID))
            {
                SaveManager.Instance._data._collectedPickups.Add(_uiniqueID);
            }
        }
        StartCoroutine(RunDialogue());
    }
    private IEnumerator RunDialogue()
    {
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.enabled = false;
        EnemyAI ai = null;
        if (_enemyAgent != null)
        {
            ai = _enemyAgent.GetComponent<EnemyAI>();
            if (ai != null) ai.enabled = false;
        }
        yield return StartCoroutine(DialogueManager.Instance.PlayDialogue(_inkJson, _namePG.Name));
        if (player != null) player.enabled = true;
        if (ai != null) ai.enabled = true;
        if (_isOneShot)
        {
            Destroy(gameObject);
        }
        else
        {
            _hasPlayed = false;
        }
    }
}
