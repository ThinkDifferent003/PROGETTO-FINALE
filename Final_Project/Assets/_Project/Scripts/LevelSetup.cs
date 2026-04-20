using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && _spawnPoint != null)
        {
            player.transform.position = _spawnPoint.transform.position;
        }
    }
}
