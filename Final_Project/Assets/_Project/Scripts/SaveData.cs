using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public string _sceneName;
    public float[] _playerPosition = new float[3];
    public int _health;
    public int _mana;
    public List<string> _inventoryID = new List<string>();
    public List<string> _enemyID = new List<string>();

    
}
