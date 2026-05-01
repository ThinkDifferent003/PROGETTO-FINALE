using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private GameObject _dropPref;
    [SerializeField] private Transform _dropPoint;

    public void DropItem()
    {
        if (_dropPref != null)
        {
            Vector3 spawnPos = _dropPoint != null? _dropPoint.position : transform.position;
            Instantiate(_dropPref, spawnPos, Quaternion.identity);
        }
    }
}
