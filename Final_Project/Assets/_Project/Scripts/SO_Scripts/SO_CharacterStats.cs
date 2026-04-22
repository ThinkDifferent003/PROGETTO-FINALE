using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "CharacterStats")]
public class SO_CharacterStats : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _attack;

    public string Name => _name;
    public int MaxHealth => _maxHealth;
    public int Attack => _attack;
}
