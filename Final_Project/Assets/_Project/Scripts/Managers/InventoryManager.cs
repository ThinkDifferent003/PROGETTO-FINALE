using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static event Action OnInventoryChanged;
    [SerializeField] private List<SO_Item> _allitems;

    private Dictionary<SO_Item , int> _objects = new Dictionary<SO_Item , int>();
    public static InventoryManager Instance => _instance;
    public Dictionary<SO_Item , int> Objects => _objects;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void AddItem(SO_Item item)
    {
        if (_objects.ContainsKey(item))
        {
            _objects[item]++;
        }
        else
        {
            _objects.Add(item, 1);
        }
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(SO_Item item)
    {
        if (item != null && _objects.ContainsKey(item)) 
        {
            _objects[item]--;
            if (_objects[item] <= 0)
            {
                _objects.Remove(item);
            }

        }
        OnInventoryChanged?.Invoke();
    }
    public int GetQuantity(SO_Item item)
    {
        if (item != null && _objects.ContainsKey(item))
        {
            return _objects[item];
        }
        return 0;
    }
    public void UpdateUI()
    {
        UI_Item[] allSlot = FindObjectsOfType<UI_Item>();
        foreach (UI_Item slot in allSlot)
        {
            slot.Refresh();
        }
    }
    public void LoadInventory(List<InventoryItemSave> savedItems)
    {
        _objects.Clear();
        foreach (var itemSave in savedItems)
        {
            SO_Item foundItem = _allitems.Find(x => x.name == itemSave._itemName);
            if (foundItem != null)
            {
                _objects.Add(foundItem, itemSave._quantity);
            }
            else
            {
                Debug.LogError($"Oggetto '{itemSave._itemName}' non trovato nel database dell'InventoryManager!");
            }
        }
        OnInventoryChanged?.Invoke();
    }
}
