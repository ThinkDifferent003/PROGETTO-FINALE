using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveData _data;
    private string _savePath;
    public bool _loadingFromMenu = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

            _savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void PerformSave()
    {
        if (_data == null) _data = new SaveData();
        _data._sceneName = SceneManager.GetActiveScene().name;
        
        var player = FindObjectOfType<PlayerLife>();
        if (player != null)
        {
            _data._health = player.GetCurrentHealth();
            _data._playerPosition = new float[]
                {player.transform.position.x,
                 player.transform.position.y,
                 player.transform.position.z};

        }
        var allEnemies = FindObjectsOfType<EnemyLife>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.IsDead)
            {
                _data._enemyID.Add(enemy.ID);
            }
        }
        SaveGame(_data);

    }

    public void PerformLoad(bool usePosition)
    {
        _data = LoadGame();

        if (_data == null) return;

        var player = FindObjectOfType<PlayerLife>();
        if (player != null)
        {
            player.SetHealth(_data._health);
            if (usePosition)
            {
                player.transform.position = new Vector3(_data._playerPosition[0], _data._playerPosition[1], _data._playerPosition[2]);

            }
        }
       
        var allEnemies = FindObjectsOfType<EnemyLife>(true);
        foreach (var enemy in allEnemies)
        {
             if (_data._enemyID.Contains(enemy.ID)) enemy.gameObject.SetActive(false);            
        }   
        
    }
    public void LoadFromMenu()
    {
        _data = LoadGame();
        if (_data != null)
        {
            PositionManager.SetNextPosition(PositionManager.SpawnType.SAVE_POINT);
            SceneManager.LoadScene(_data._sceneName);
        }
    }
    public void SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            Debug.Log($"Gioco salvato in : {_savePath}");
        }
        catch (System.Exception e)
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
