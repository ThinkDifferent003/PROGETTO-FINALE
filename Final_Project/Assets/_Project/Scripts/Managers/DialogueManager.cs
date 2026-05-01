
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {  get; private set; }

    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _namePanel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private float _waitForSeconds = 1f;
    [SerializeField] private float _typingSpeed = 0.05f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public IEnumerator PlayDialogue(TextAsset inkJson, string namePG)
    {
        Story story = new Story(inkJson.text);
        _nameText.text = namePG;
        _namePanel.SetActive(true);
        _dialoguePanel.SetActive(true);
        while(story.canContinue)
        {
            string fullLine = story.Continue();
            yield return StartCoroutine(TypeText(fullLine));
            yield return new WaitForSeconds(_waitForSeconds);
        }
        _dialoguePanel.SetActive(false);
        _namePanel.SetActive(false);
    }
    private IEnumerator TypeText(string line)
    {
        _dialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }
    }
}

