using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class ClashManager : MonoBehaviour
{
    public static ClashManager Instance;

    [SerializeField] private float _slowMotion = 0.1f;
    [SerializeField] private float _duration = 0.2f;
    //[SerializeField] private GameObject _clashEffect;

    private void Awake()
    {
        Instance = this;
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        TriggerClash(Vector3.up * 2);
    //    }
    //}
    private IEnumerator ClashRoutine(Vector3 position)
    {
        //if (_clashEffect  != null)
        //{
        //    Instantiate(_clashEffect,position,Quaternion.identity);
        //}
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
