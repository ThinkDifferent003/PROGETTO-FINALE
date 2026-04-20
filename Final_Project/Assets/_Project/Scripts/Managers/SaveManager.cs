using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string _savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
       
        _savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }
    
    public void PerformSave()
    {
        SaveData data = new SaveData();
        data._sceneName = SceneManager.GetActiveScene().name;
        var player = FindObjectOfType<PlayerLife>();
        if (player != null)
        {
            data._health = player.GetCurrentHealth();
            data._playerPosition = new float[]
                {player.transform.position.x,
                 player.transform.position.y,
                 player.transform.position.z};

        }
        var allEnemies = FindObjectsOfType<EnemyLife>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.IsDead)
            {
                data._enemyID.Add(enemy.ID);
            }
        }
        SaveGame(data);
    }

    public void PerformLoad()
    {
        SaveData data = LoadGame();
       
        if (data ==  null) return;
        var player = FindObjectOfType<PlayerLife>();
        if (player != null)
        {
            player.transform.position = new Vector3(data._playerPosition[0], data._playerPosition[1], data._playerPosition[2]);
            player.SetHealth(data._health);
        }
        var allEnemies = FindObjectsOfType<EnemyLife>();
        foreach(var enemy in allEnemies)
        {
            if (data._enemyID.Contains(enemy.ID))
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
    public void LoadFromMenu()
    {
        SaveData data = LoadGame();
        if (data !=  null) 
        {
            
            SceneManager.LoadScene(data._sceneName);
            StartCoroutine(WaitAndLoad(data));
            
        }
    }
    private IEnumerator WaitAndLoad(SaveData data)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == data._sceneName);
        PerformLoad();
    }
    public void SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            Debug.Log($"Gioco salvato in : {_savePath}");
        }
        catch(System.Exception e)
        {
            Debug.Log($"Errore : {e.Message}");
        }
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log($"Nessun file trovato");
            return null;
        }
        try
        {
            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Errore {e.Message}");
            return null;
        }
    }
}
