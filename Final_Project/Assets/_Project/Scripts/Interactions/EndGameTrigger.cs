using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [Header("EndGame References")]
    [SerializeField] private IntroManager _endGame;
    [SerializeField] private GameObject _endPanel;

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasTriggered)
        {
            if (_endGame != null)
            {
                _hasTriggered = true;
                if (_endPanel != null) _endPanel.SetActive(true);
                _endGame.StartIntro();
            }
        }
    }
}
