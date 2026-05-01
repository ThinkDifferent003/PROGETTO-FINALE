using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public static event Action OnInventoryChanged;

    [Header("Database")]
    [SerializeField] private List<SO_Item> _allitems;

    private Dictionary<SO_Item , int> _objects = new Dictionary<SO_Item , int>(); 
    public Dictionary<SO_Item , int> Objects => _objects;

    private void Awake()
    {
        InizializeSingleton();
    }
    private void InizializeSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);   
    }
    public void AddItem(SO_Item item)
    {
        if (_objects.ContainsKey(item)) _objects[item]++;   
        else _objects.Add(item, 1);   
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(SO_Item item)
    {
        if (item != null && _objects.ContainsKey(item)) 
        {
            _objects[item]--;
            if (_objects[item] <= 0) _objects.Remove(item);
        }
        OnInventoryChanged?.Invoke();
    }
    public int GetQuantity(SO_Item item)
    {
        if (item != null && _objects.ContainsKey(item)) return _objects[item];
        return 0;
    }
    public void LoadInventory(List<InventoryItemSave> savedItems)
    {
        _objects.Clear();
        if (savedItems == null) return;
        foreach (var itemSave in savedItems)
        {
            SO_Item foundItem = _allitems.Find(x => x.name == itemSave._itemName);
            if (foundItem != null) _objects.Add(foundItem, itemSave._quantity);         
        }
        OnInventoryChanged?.Invoke();
    }
}
