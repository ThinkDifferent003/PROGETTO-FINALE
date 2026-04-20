using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private float _open = 90f;
    [SerializeField] private float _smooth = 2f;
    private bool _isOpen = false;
    private bool _isPlayerNearby = false;
    private Quaternion _closeRotation;
    private Quaternion _targetRotation;

    private void Start()
    {
        _closeRotation = transform.localRotation;
        _targetRotation = _closeRotation;
    }

    private void Update()
    {
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.E))
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
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, _smooth * Time.deltaTime);
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
}
