using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Header("World State")]
    public string _sceneName;
    public string _spawnName;

    [Header("Player State")]
    public float[] _playerPosition = new float[3];
    public int _health;

    [Header("Inventory & Progression State")]
    public List<InventoryItemSave> _inventoryID = new List<InventoryItemSave>();
    public List<string> _collectedPickups = new List<string>();

    [Header("Enemy State")]
    public List<string> _enemyID = new List<string>();
}
