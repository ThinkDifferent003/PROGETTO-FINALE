using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private Image _healthBar;
    private void OnEnable()
    {
        PlayerLife.OnHealthChanged += UpdateFill;
    }
    private void OnDisable()
    {
        PlayerLife.OnHealthChanged -= UpdateFill;
    }
    private void UpdateFill(int current,int max)
    {
        if (_healthBar != null && max > 0) _healthBar.fillAmount = (float)current / max;
        
    }
}
