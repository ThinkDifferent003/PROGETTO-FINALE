using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class ClashManager : MonoBehaviour
{
    public static ClashManager Instance { get; private set; }

    [Header("Time Settings")]
    [SerializeField] private float _slowMotion = 0.1f;
    [SerializeField] private float _duration = 0.2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    } 
    private IEnumerator ClashRoutine(Vector3 position)
    {
        Time.timeScale = _slowMotion;
        yield return new WaitForSecondsRealtime(_duration);
        Time.timeScale = 1f;
        Debug.Log("Clash!");
    }
    public void TriggerClash(Vector3 position)
    {
        StopAllCoroutines();
        StartCoroutine(ClashRoutine(position));
    }
}
