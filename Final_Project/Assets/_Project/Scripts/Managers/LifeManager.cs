using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LifeManager : MonoBehaviour , IDamageable
{
    [SerializeField] private SO_CharacterStats _stats;
    protected int _currentHealth;
    [SerializeField] private string _id;


    public int GetMaxHealth() => _stats.MaxHealth;
    public int GetCurrentHealth() => _currentHealth;
    public void SetHealth(int amount) => _currentHealth = amount;
    public string ID
    {
        get
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = gameObject.name +"_" + transform.position.ToString();
            }
            return _id;
        }
    }
    public bool IsDead
    {
        get
        {
            return _currentHealth <= 0;
        }
    }

    protected virtual  void Start()
    {
        
        if (_currentHealth == 0 && _stats !=  null)
        {
            _currentHealth = _stats.MaxHealth;
        }
    }
    
    protected abstract void Die();
    protected abstract void ApplyRecoil(Vector3 direction);
    public virtual void TakeDamage(int damage , Vector3 attacker)
    {
        if (IsDead) return;
        _currentHealth -= damage;
        Vector3 recoilDir = (transform.position - attacker).normalized;
        recoilDir.y = 0;
        Debug.Log("LifeManager: Applico Recoil");
        ApplyRecoil(recoilDir);
        Debug.Log($"{gameObject.name}. Ha ricevuto {damage} danni. HP = {_currentHealth}");
        if (IsDead)
        {
            Die();
        }
    }
}
