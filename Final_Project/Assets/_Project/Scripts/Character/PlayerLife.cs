using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : LifeManager
{
    private PlayerAnimation _playerAnimation;
    private PlayerController _playerController;
    public static event Action<int, int> OnHealthChanged;

    protected override void Start()
    {
        base.Start();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _playerController = GetComponent<PlayerController>();
        UpdateUI();
    }
    private void UpdateUI()
    {
        OnHealthChanged?.Invoke(_currentHealth, GetMaxHealth());
    }
    protected override void Die()
    {
        if (_playerAnimation != null)
        {
            _playerAnimation.AnimationDie();
        }
    }
    public override void TakeDamage(int damage, Vector3 attacker)
    {
        if (IsDead) return;
        base.TakeDamage(damage,attacker);
        if (!IsDead)
        {
            _playerAnimation.AnimationHurt();
        }
        UpdateUI();
    }
    protected override void ApplyRecoil(Vector3 direction)
    {
        if (_playerController != null && !IsDead)
        {
            StartCoroutine(_playerController.Recoil(direction));
        }
    }
    public void Heal(int heal)
    {
        _currentHealth += heal;
        _currentHealth = Mathf.Clamp(_currentHealth,0,GetMaxHealth());
        UpdateUI();
    }
}
