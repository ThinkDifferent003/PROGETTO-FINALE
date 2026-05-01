using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _blackPanel;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Animator _animator;

    [Header("Intro Settings")]
    [SerializeField] private string[] _storyLines;
    [SerializeField] private float _typingSpeed = 0.1f;

    [Header("Transiction Settings")]
    [SerializeField] private GameObject _gameManagerPref;
    [SerializeField] private string _sceneToLoad = "Level1";

    private int _typingCount = 0;
    private bool _isTyping = false;
    private bool _isIntroActive = false;

    public void StartIntro()
    {
        _blackPanel.SetActive(true);
        _isIntroActive = true;
        _typingCount = 0;
        _text.text = "";
        StartCoroutine(StartIntroRoutine());

    }
    private void Update()
    {
        if (!_isIntroActive) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (_isTyping)
            {
                 StopAllCoroutines();
                _text.text = _storyLines[_typingCount];
                _isTyping = false;
            }
            else NextSentence();
        }
    }
    private IEnumerator StartIntroRoutine()
    {
        if (_animator != null) _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        StartCoroutine(TypeSentence(_storyLines[0]));
    }
    private IEnumerator TypeSentence(string sentence)
    {
        _isTyping = true;
        StringBuilder builder = new StringBuilder();
        _text.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            builder.Append(letter);
            _text.text = builder.ToString();
            yield return new WaitForSeconds(_typingSpeed);
        }
        _isTyping = false;
    }
    private void NextSentence()
    {
        _typingCount++;
        if (_typingCount < _storyLines.Length) StartCoroutine(TypeSentence(_storyLines[_typingCount])); 
        else StartCoroutine(EndIntro());
    }
    private IEnumerator EndIntro()
    {
        _isIntroActive = false;
        _text.text = "";
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(_sceneToLoad);
        if (_sceneToLoad == "Level1")
        {
            GameObject manager = Instantiate(_gameManagerPref);
            DontDestroyOnLoad(manager);
        } 
    }
}
