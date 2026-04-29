using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevel(string levelName , string spawnName)
    {
        Debug.Log("--- INIZIO PROCEDURA CAMBIO SCENA ---");
        
        if (SaveManager.Instance != null) 
        {
            SaveManager.Instance.PerformSave();
            Debug.Log("--- SALVATAGGIO COMPLETATO ---");
        }
        else
        {
            Debug.LogError("--- ERRORE: SaveManager.Instance è NULL! ---");
        }

        PositionManager.SetNextPosition(PositionManager.SpawnType.PORTAL, spawnName);
        SceneManager.LoadScene(levelName);
    }
}
