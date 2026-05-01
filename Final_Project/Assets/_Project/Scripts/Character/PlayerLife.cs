using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : LifeManager
{
    [Header("UI Reference")]
    [SerializeField] GameObject _gameOverPanel;

    private PlayerAnimation _playerAnimation;
    private PlayerController _playerController;

    public static event Action<int, int> OnHealthChanged;
 
    protected override void Awake()
    {
        base.Awake();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _playerController = GetComponent<PlayerController>();
        if (_gameOverPanel != null ) _gameOverPanel.SetActive(false);
        UpdateUI();
    }
    #region Life
    public override void TakeDamage(int damage, Vector3 attacker)
    {
        if (IsDead) return;
        base.TakeDamage(damage,attacker);
        //Se dopo il colpo è ancora vivo
        if (!IsDead && _playerAnimation != null) _playerAnimation.AnimationHurt(); //Avvio animazione di urto      
        UpdateUI(); //Aggiorna la vita
    }
    public override void SetHealth(int amount)
    {
        base.SetHealth(amount);
        UpdateUI();
    }
   
    public void Heal(int heal)
    {
        _currentHealth += heal; //Aggiunge vita
        _currentHealth = Mathf.Clamp(_currentHealth,0,GetMaxHealth()); //Mi assicuro che gli HP non superino la vita max
        UpdateUI(); //Aggiorno UI
    }
    protected override void ApplyRecoil(Vector3 direction)
    {
        if (_playerController != null && !IsDead) StartCoroutine(_playerController.Recoil(direction)); //Parte la coroutine    
    }
    protected override void Die()
    {
        if (_playerController != null) _playerController.enabled = false;
        if (_playerAnimation != null) _playerAnimation.AnimationDie();
        StartCoroutine(ShowGameOverPanel());
    }
    #endregion
    #region Events & UI
    private IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }   
    private void UpdateUI()
    { 
        OnHealthChanged?.Invoke(_currentHealth, GetMaxHealth());
    }
    public void UpdateUIExternally() => UpdateUI();
    #endregion
    #region Reset Logic
    public void Reset()
    {
        if (_playerController != null) _playerController.enabled = true;
        if (_playerAnimation != null)
        {
            Animator anim = _playerAnimation.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("Idle", 0, 0f);
                anim.Rebind();
                anim.Update(0f);
            }
        }
        UpdateUI();
    }
    #endregion
}
