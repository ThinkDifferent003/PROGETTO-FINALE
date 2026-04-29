using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField] private GameObject _letterPanel;
    [SerializeField] private TextMeshProUGUI _textLetter;
    private bool _isPlayerRange;

    private void Update()
    {
        if (_isPlayerRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenLetter();
        }
    }
    private void OpenLetter()
    {
        _letterPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseLetter()
    {
        _letterPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isPlayerRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isPlayerRange = false;
        }
    }
}
