using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] private IntroManager _endGame;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasTriggered)
        {
            if (_endGame != null)
            {
                _hasTriggered = true;
                _endGame.StartIntro();
            }
        }
    }
}
