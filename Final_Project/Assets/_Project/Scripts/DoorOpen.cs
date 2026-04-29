using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private float _open = 90f;
    [SerializeField] private float _smooth = 2f;
    [SerializeField] private bool _isRequireKey = false;
    [SerializeField] private SO_Item _key;
    private bool _isOpen = false;
    private bool _isPlayerNearby = false;
    private Quaternion _closeRotation;
    private Quaternion _targetRotation;
    [SerializeField] private SO_CharacterStats _characterName;
    [SerializeField] private TextAsset _dialogueNoKey;
    private bool _isDialoguePlay = false;

    private void Start()
    {
        _closeRotation = transform.localRotation;
        _targetRotation = _closeRotation;
    }

    private void Update()
    {
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TryOpen();
               
        }
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, _smooth * Time.deltaTime);
    }
    private bool HasKey()
    {
        if (!_isRequireKey) return true;
        if (_key == null) return false;
        if (InventoryManager.Instance != null)
        {
            return InventoryManager.Instance.GetQuantity(_key) > 0;
        }
        return false;
    }
    private void Open()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            _targetRotation = Quaternion.Euler(0, _open, 0) * _closeRotation;
        }
        else
        {
            _targetRotation = _closeRotation;
        }
    }
    private void TryOpen()
    {
        if (_isOpen)
        {
            Open();
            return;
        }
        if (_isRequireKey)
        {
            if (HasKey())
            {
                Debug.Log("Apro porta con chiave");
                InventoryManager.Instance.RemoveItem(_key);
                _isRequireKey = false;
                Open();
            }
            else
            {
                if (_dialogueNoKey !=  null)
                {
                    StartCoroutine(TriggerDialogue(_dialogueNoKey));
                }
            }
        }
        else
        {
            Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
        }
    }
    private IEnumerator TriggerDialogue(TextAsset inkFile)
    {
        _isDialoguePlay = true;
        yield return StartCoroutine(DialogueManager.Instance.PlayDialogue(inkFile, _characterName.Name));
        _isDialoguePlay = false;
    }
}
