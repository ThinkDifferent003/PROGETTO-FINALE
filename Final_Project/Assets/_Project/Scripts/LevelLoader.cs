using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    private void Awake()
    {
        InizializeSingleton();
    }
    private void InizializeSingleton()
    {
        if (Instance == null) Instance = this; 
        else Destroy(gameObject);   
    }
    public void LoadLevel(string levelName , string spawnName)
    { 
        if (string.IsNullOrEmpty(levelName)) return;
        if (SaveManager.Instance != null) SaveManager.Instance.PerformSave();
        PositionManager.SetNextPosition(PositionManager.SpawnType.PORTAL, spawnName);
        SceneManager.LoadScene(levelName);
    }
}
