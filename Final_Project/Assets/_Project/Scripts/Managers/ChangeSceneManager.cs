using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    [Header("Destination Settings")]
    [SerializeField] private string _sceneName;
    [SerializeField] private string _spawnID;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Qualcosa è entrato nel trigger: " + other.name);
        if (other.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(_sceneName)) return;
            LevelLoader.Instance.LoadLevel(_sceneName, _spawnID);
        }
    }
}
