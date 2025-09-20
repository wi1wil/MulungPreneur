using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentScript : MonoBehaviour
{
    public EquipmentsSO bags;
    public EquipmentsSO shoes;
    public EquipmentsSO gloves;
    public EquipmentsSO tools;

    private int bagLevel, shoeLevel, gloveLevel, toolLevel;

    private PlayerCurrencyScript playerCurrency;

    void Start()
    {
        playerCurrency = GetComponent<PlayerCurrencyScript>();
        updateAll();
    }

    public void updateAll()
    {
        updateEquipment(bags, bagLevel);
        updateEquipment(shoes, shoeLevel);
        updateEquipment(gloves, gloveLevel);
        updateEquipment(tools, toolLevel);
    }

    public void updateEquipment(EquipmentsSO equipment, int level)
    {

    }

    public void upgradeEquipment(EquipmentsSO equipment)
    {

    }
}
