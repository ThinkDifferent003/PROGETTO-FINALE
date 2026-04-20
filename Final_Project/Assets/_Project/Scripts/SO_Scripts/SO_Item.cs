using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuovoOggetto", menuName = "Oggetto")]
public class SO_Item : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _effect;
    [SerializeField] private bool _isConsumable;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    public int Effect => _effect;
    public bool IsConsumable => _isConsumable;
}
