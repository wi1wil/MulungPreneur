using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Transform _inventoryPanel;
    [SerializeField] private GameObject _slotPrefab;

    private List<InventorySlot> _slots = new();

    void Start()
    {
        InventoryManager.Instance.onInvChanged += RefreshUI;
        GenerateUISlots();
    }

    void GenerateUISlots()
    {
        for (int i = 0; i < InventoryManager.Instance.GetInventory().Count; i++)
        {
            Debug.Log("Generating UI Slots");
            var slot = Instantiate(_slotPrefab, _inventoryPanel);
            var uiSlot = slot.GetComponent<InventorySlot>();
            uiSlot.SetIndex(i);
            _slots.Add(uiSlot);
        }
        RefreshUI();
    }
    
    void RefreshUI()
    {
        var inventory = InventoryManager.Instance.GetInventory();
        if (_slots.Count != inventory.Count)
        {
            foreach (var slot in _slots)
                Destroy(slot.gameObject);
            _slots.Clear();
            GenerateUISlots();
            return;
        }
        
        for(int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetItem(inventory[i]);
        }
    }
}
