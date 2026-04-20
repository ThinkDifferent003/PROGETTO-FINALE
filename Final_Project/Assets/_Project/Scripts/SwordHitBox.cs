using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField] private SO_CharacterStats _stats;
    
    private List<IDamageable> _alreadyHit = new List<IDamageable>();
    public GameObject Owner => transform.root.gameObject;
    private bool _colliderEnabled = false;

    private void Update()
    {
        Collider col = GetComponent<Collider>();
        if (!_colliderEnabled && col.enabled)
        {
            ResetHitbox();
        }
        _colliderEnabled = col.enabled;
    }
    public void ResetHitbox()
    {
        _alreadyHit.Clear();
        Debug.Log("Hitbox Resettsata per un nuovo attacco");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        SwordHitBox otherSword = other.GetComponent<SwordHitBox>();

        if (otherSword != null)
        {
            if (otherSword.Owner != this.Owner)
            {
                ClashManager.Instance.TriggerClash(transform.position);
                return;
            }
        }
            
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !_alreadyHit.Contains(damageable))
        {
            damageable.TakeDamage(_stats.Attack , Owner.transform.position);
            _alreadyHit.Add(damageable);
            Debug.Log("Colpito" + other.name);
        }
    }
}
