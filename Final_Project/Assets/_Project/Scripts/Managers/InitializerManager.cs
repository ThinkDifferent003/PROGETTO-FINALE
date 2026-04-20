using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializerManager : MonoBehaviour
{
    [SerializeField] private bool _isNewGame = false;
    private void Start()
    {
        if (_isNewGame)
        {
            SpawnAtScene();
        }
        else
        {
            Debug.Log("Sta chiamando i dati");
            SaveManager.Instance.PerformLoad();
        }
            
    }
    private void SpawnAtScene()
    {
        GameObject spawnPoint = GameObject.Find("PlayerSpawn");
        if (spawnPoint != null )
        {
            var player = FindObjectOfType<PlayerLife>();
            if ( player != null )
            {
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }
}
