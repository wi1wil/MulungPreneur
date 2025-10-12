using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShopKeeper : MonoBehaviour, IInteractable
{
    public bool RequiresHold => false;
    private EquipmentUI _equipmentUI;
    private ShopUIManager _shopUIManager;

    [SerializeField] private GameObject _shopPanel;

    private void Start()
    {
        _equipmentUI = FindAnyObjectByType<EquipmentUI>();
        _shopUIManager = FindAnyObjectByType<ShopUIManager>();
    }

    public void Interact()
    {
        MenuPauseManager.instance.TogglePause();
        if (_shopPanel.activeSelf)
        {
            _shopPanel.SetActive(false);
        }
        else
        {
            _shopUIManager.PopulateShopItems(InventoryManager.Instance.GetInventory());
            _equipmentUI.UpdateAllUI();
            _shopPanel.SetActive(true);
        }
    }
}
