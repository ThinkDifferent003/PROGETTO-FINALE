using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    [Header("Item Configuration")]
    [SerializeField] private SO_Item _object;
    [SerializeField] private KeyCode _keyCode;

    [Header("UI References")]
    [SerializeField] private Image _imageIcon;  
    [SerializeField] private GameObject _iconObject;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_Text _keyUseItem;

    private void Awake()
    {
        if (_iconObject  != null) _imageIcon = _iconObject.GetComponent<Image>();
        if (_object != null && _imageIcon != null) _imageIcon.sprite = _object.Icon;
        if (_keyUseItem != null) _keyUseItem.text = _keyCode.ToString();
    }
    private void Start()
    {
        Refresh();
    }
    private void Update()
    {
        if (Input.GetKeyDown(_keyCode)) UseItem();
    }
    private void OnEnable()
    {
        InventoryManager.OnInventoryChanged += Refresh;
    }
    private void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= Refresh;
    }
    public void Refresh()
    {
        if (InventoryManager.Instance ==  null || _object == null) return; 
        int quantity = InventoryManager.Instance.GetQuantity(_object);
        if (quantity > 0) _iconObject.SetActive(true);
        else _iconObject.SetActive(false);
        if (_quantityText != null)
        {
            _quantityText.text = quantity.ToString();
            _quantityText.gameObject.SetActive(quantity > 0);
        }
    }
    public void UseItem()
    {
        if (InventoryManager.Instance.GetQuantity( _object) <= 0) return;
        if (_object.IsConsumable)
        {
            PlayerLife player = FindFirstObjectByType<PlayerLife>();
            if (player != null)
            {
                player.Heal(_object.Effect);
                InventoryManager.Instance.RemoveItem(_object);
            }
        }
    }
}
