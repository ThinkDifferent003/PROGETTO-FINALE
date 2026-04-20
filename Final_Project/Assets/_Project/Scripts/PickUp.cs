using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickUp : MonoBehaviour
{
    [SerializeField] private SO_Item _itemData;
    private bool _isPlayerNearby = false;
    
    private void Update()
    {
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }
    private void PickUpItem()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(_itemData);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
        }
    }
}
