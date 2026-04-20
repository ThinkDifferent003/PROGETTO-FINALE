using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
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
        SaveManager.Instance.PerformSave();
        Debug.Log("Gioco salvato");
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void SaveAndQuit()
    {
        SaveManager.Instance.PerformSave();
        SceneManager.LoadScene("MainMenu");
    }
}
