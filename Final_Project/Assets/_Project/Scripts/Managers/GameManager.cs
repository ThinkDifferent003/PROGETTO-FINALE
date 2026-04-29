using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    {
        CleanScene();
    }
    public void CleanScene()
    {
        if (SaveManager.Instance == null || SaveManager.Instance._data == null) return;
        PickUp[] allPickups = FindObjectsOfType<PickUp>(true);
        foreach (PickUp pickup in allPickups)
        {
            if (SaveManager.Instance._data._collectedPickups.Contains(pickup.UniqueID()))
            {
                Destroy(pickup.gameObject);
            }
        }
        EnemyLife[] allEnemies = FindObjectsOfType<EnemyLife>(true);
        foreach (EnemyLife enemy in allEnemies)
        {
            if (SaveManager.Instance._data._enemyID.Contains(enemy.ID))
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
