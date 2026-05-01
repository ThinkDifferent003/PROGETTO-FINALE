using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : LifeManager
{
    private EnemyAnimation _enemyAnimation;
    private EnemyAI _enemyController;
    private LootDrop _lootDrop;

    protected override void Awake()
    {
        base.Awake();
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        _enemyController = GetComponent<EnemyAI>();
        _lootDrop = GetComponent<LootDrop>();
    }
    public override void TakeDamage(int damage,Vector3 attacker)
    {
        if (IsDead) return;
        base.TakeDamage(damage, attacker);
        if (!IsDead)
        {
            _enemyAnimation.AnimationHurt();
        }
    }
    protected override void Die()
    {
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null) 
        {
            enemyAI.enabled = false;
        }
        if (_lootDrop != null) _lootDrop.DropItem();

        if (_enemyAnimation != null)
        {
            _enemyAnimation.AnimationDie();
            //Destroy(gameObject, 2f);
        }
    }
    protected override void ApplyRecoil(Vector3 direction)
    {
        if (IsDead) return;
        if (_enemyController != null) 
        {
            StartCoroutine(_enemyController.Recoil(direction));
        }
    }
    
}
