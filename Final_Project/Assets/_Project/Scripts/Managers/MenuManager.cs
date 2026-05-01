using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _menuPanel;
    
    private bool _isPaused = false;
    private void Start()
    {
        Time.timeScale = 1f;
        if (_menuPanel != null) _menuPanel.SetActive(false);
        _isPaused = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();  
        }
    }
    public void Pause()
    {
        _menuPanel.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
    public void Resume()
    {
        _menuPanel.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void SaveGame()
    {
        if (SaveManager.Instance != null) SaveManager.Instance.PerformSave();
        Debug.Log("Gioco salvato");
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
    public void SaveAndQuit()
    {
        Resume();
        SaveGame();
        QuitGame();
    }
    
}
