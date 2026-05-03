using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PickUp : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private SO_Item _itemData;
    [SerializeField] private string _uniqueID;

    private bool _isPlayerNearby = false;

    public string UniqueID() {  return _uniqueID; }
    private void Start()
    {
        if (SaveManager.Instance != null && SaveManager.Instance._data != null)
        {
            if (SaveManager.Instance._data._collectedPickups.Contains(_uniqueID)) Destroy(gameObject);     
        }
    }
    private void Update()
    {
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.E)) PickUpItem();    
    }
    private void PickUpItem()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(_itemData);
            if (SaveManager.Instance != null && SaveManager.Instance._data != null)
            {
                if (!SaveManager.Instance._data._collectedPickups.Contains(_uniqueID)) SaveManager.Instance._data._collectedPickups.Add(_uniqueID);   
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isPlayerNearby = true;  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _isPlayerNearby = false;    
    }
}
