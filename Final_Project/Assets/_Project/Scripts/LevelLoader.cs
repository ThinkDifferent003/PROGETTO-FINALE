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
        SaveManager.Instance.PerformSave();
        SceneManager.LoadScene(levelName);
        PlayerPrefs.SetString("LastSpawn" , spawnName);
    }
}
