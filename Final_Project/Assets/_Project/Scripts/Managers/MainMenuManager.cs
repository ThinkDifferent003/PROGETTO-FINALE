using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string _gameSceneName = "Scena1";
    [SerializeField] private GameObject _gameManagerPref;

    [Header("Intro Settings")]
    [SerializeField] private IntroManager _introManager;

    [Header("Intro Panels")]
    [SerializeField] private GameObject _creditPanel;
    [SerializeField] private Button _loadButton;

    public static bool _intro = false;

    private void Start()
    {
        CheckSave();
    }
    private void CheckSave()
    {
        if (_loadButton  != null)
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
            bool hasSave = File.Exists(path);
            _loadButton.interactable = hasSave;
        }
    }
    public void NewGame()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
        if (File.Exists(path)) File.Delete(path);
        _intro = true;
        if (_introManager  != null) _introManager.StartIntro();
        else
        {
            Instantiate(_gameManagerPref);
            SceneManager.LoadScene(_gameSceneName);
        }
            
    }
    public void LoadGame()
    {
        _intro = false;
        if (SaveManager.Instance != null) SaveManager.Instance.LoadFromMenu();
        if (_gameManagerPref  != null)
        {
            GameObject manager = Instantiate(_gameManagerPref);
            DontDestroyOnLoad(manager);
        }    
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenCredits()
    {
        _creditPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        _creditPanel.SetActive(false);
    }
}
