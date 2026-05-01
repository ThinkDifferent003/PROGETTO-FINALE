using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuovoOggetto", menuName = "Oggetto")]
public class SO_Item : ScriptableObject
{
    [Header("general Info")]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;

    [Header("Settings")]
    [SerializeField] private int _effect;
    [SerializeField] private bool _isConsumable;
    
    public Sprite Icon => _icon;
    public int Effect => _effect;
    public bool IsConsumable => _isConsumable;
}
