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
        if (_shopPanel.activeSelf)
        {
            Close();
        }
        else
        {
            AudioManager.instance.PlayUIClick();
            MenuPauseManager.instance.SetPaused(true);
            _shopUIManager.PopulateShopItems(InventoryManager.Instance.GetInventory());
            _equipmentUI.UpdateAllUI();
            _shopPanel.SetActive(true);
        }
    }

    public void Close()
    {
        MenuPauseManager.instance.SetPaused(false);
        _shopPanel.SetActive(false);
        _shopUIManager.ClearBuyback();
    }
}
