using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [SerializeField] private GameObject _HUD;
    [SerializeField] private UI_HealthBar _HealthBar;
    [SerializeField] private UI_Item _slot1;
    [SerializeField] private UI_Item _slot2;

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
    }
}
