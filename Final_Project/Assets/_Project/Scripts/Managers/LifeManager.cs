using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LifeManager : MonoBehaviour , IDamageable
{
    [SerializeField] private SO_CharacterStats _stats;
    [SerializeField] private string _id; //ID utile per i salvataggi
    protected int _currentHealth; //Vita corrente. Protected cosi le figlie possono accedere
    


    public int GetMaxHealth() => _stats.MaxHealth;
    public int GetCurrentHealth() => _currentHealth;
    public virtual void SetHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(amount, 0, GetMaxHealth()); 
    }
        
    
    public string ID
    {
        get
        {
            if (string.IsNullOrEmpty(_id)) //Se vuoto, ne genero uno basato su nome e posizione
            {
                _id = gameObject.name +"_" + transform.position.ToString();
            }
            return _id;
        }
    }
    public bool IsDead //Controllo se è morto
    {
        get
        {
            return _currentHealth <= 0;
        }
    }

    protected virtual void Awake()
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
        if (IsDead) return; //Se già morto , esco

        _currentHealth -= damage; //Sottrazione del danno
        //Calcolo recoil    (  Mia posizione  )  (Attaccante) = Spinta
        Vector3 recoilDir = (transform.position - attacker).normalized;
        recoilDir.y = 0; //Impedisco che la spinta vada verso l'alto o basso
        Debug.Log("LifeManager: Applico Recoil");
        
        ApplyRecoil(recoilDir); 
        Debug.Log($"{gameObject.name}. Ha ricevuto {damage} danni. HP = {_currentHealth}");
        if (IsDead)
        {
            Die();
        }
    }
}
