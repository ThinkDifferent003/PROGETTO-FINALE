using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Retry()
    {
        Time.timeScale = 1f;
        PositionManager.SetNextPosition(PositionManager.SpawnType.DEFAULT);
        if (SaveManager.Instance !=  null && SaveManager.Instance._data != null) SaveManager.Instance._data._enemyID.Clear();
        GameObject panel = GameObject.Find("GameOverPanel");
        if (panel != null) panel.SetActive(false);
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

