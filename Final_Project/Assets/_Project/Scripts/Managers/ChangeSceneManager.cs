using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private string _spawnID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("Spawnpoint", _spawnID);
            SceneManager.LoadScene(_sceneName);
        }
    }
}
