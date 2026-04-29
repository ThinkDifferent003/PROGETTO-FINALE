using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public string _sceneName;
    public string _spawnName;
    public float[] _playerPosition = new float[3];
    public int _health;
    public int _mana;
    public List<InventoryItemSave> _inventoryID = new List<InventoryItemSave>();
    public List<string> _collectedPickups = new List<string>();
    public List<string> _enemyID = new List<string>();

    
}
