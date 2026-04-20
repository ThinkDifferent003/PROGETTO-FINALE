using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "Scena1";
    [SerializeField] private IntroManager _introManager;
    [SerializeField] private GameObject _gameManagerPref;
   

    public void NewGame()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        if (_introManager  != null)
        {
            _introManager.StartIntro();
        }
        else
        {
            Instantiate(_gameManagerPref);
            SceneManager.LoadScene(_gameSceneName);
        }
            
    }
    public void LoadGame()
    {
        SaveManager.Instance.LoadFromMenu();
        GameObject manager = Instantiate(_gameManagerPref);
        DontDestroyOnLoad(manager);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
