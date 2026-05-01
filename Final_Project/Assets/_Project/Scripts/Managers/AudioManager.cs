using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _mainClip;
    [SerializeField] private AudioClip _bossClip;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    {
        if (scene.name == "Level3")
        {
            PlayMusic(_bossClip);
        }
        else
        {
            PlayMusic(_mainClip);
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || _audioSource == null) return;
        if (_audioSource.clip == clip && _audioSource.isPlaying) return;
        _audioSource.clip = clip;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
