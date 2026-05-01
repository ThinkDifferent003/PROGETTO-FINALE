using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Music tracks")]
    [SerializeField] private AudioClip _mainClip;
    [SerializeField] private AudioClip _bossClip;

    [Header("Settings")]
    [SerializeField] private float _fade = 1.5f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    {
        if (scene.name == "Level3") PlayMusic(_bossClip);
        else PlayMusic(_mainClip);   
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || _audioSource == null) return;
        if (_audioSource.clip == clip && _audioSource.isPlaying) return;
        StopAllCoroutines();
        StartCoroutine(FadeMusic(clip));
    }
    private IEnumerator FadeMusic(AudioClip clip)
    {
        float startVolume = _audioSource.volume;
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= startVolume * Time.deltaTime / (_fade / 2);
            yield return null;
        }
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
        while (_audioSource.volume < startVolume)
        {
            _audioSource.volume += startVolume + Time.deltaTime / (_fade / 2);
            yield return null;
        }
        _audioSource.volume = startVolume;
    }
}
