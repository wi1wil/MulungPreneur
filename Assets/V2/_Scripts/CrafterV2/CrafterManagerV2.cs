using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrafterManagerV2 : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private CrafterUIManagerV2 uiManager;   
    [SerializeField] private RectTransform progressContent;
    [SerializeField] private GameObject craftingSlotPrefab; 

    private List<CrafterSlotUIV2> activeSlots = new List<CrafterSlotUIV2>();

    public bool TryCraft(RecipeSO recipe)
    {
        if (activeSlots.Count >= 3)
        {
            Debug.Log("Crafter is busy. Please wait.");
            return false;
        }
        
        if (!HasEnoughItems(recipe))
        {
            Debug.Log("Not enough resources to craft " + recipe.recipeName);
            return false;
        }

        RemoveInputs(recipe);
        StartCoroutine(CraftRoutine(recipe));
        return true;
    }

    private IEnumerator CraftRoutine(RecipeSO recipe)
    {
        var go = Instantiate(craftingSlotPrefab, progressContent);
        var slot = go.GetComponent<CrafterSlotUIV2>();
        slot.Setup(recipe, this);

        activeSlots.Add(slot);

        float timer = 0f;
        while (timer < recipe.craftTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / recipe.craftTime);
            slot.UpdateProgress(progress);
            yield return null;
        }

        slot.MarkAsFinished();
        activeSlots.Remove(slot);
    }

    private bool HasEnoughItems(RecipeSO recipe)
    {
        var counts = InventoryManager.Instance.GetItemCountBySO();
        foreach (var req in recipe.inputs)
        {
            if (!counts.ContainsKey(req.item) || counts[req.item] < req.amount)
                return false;
        }
        return true;
    }

    private void RemoveInputs(RecipeSO recipe)
    {
        foreach (var req in recipe.inputs)
        {
            InventoryManager.Instance.RemoveItem(req.item.itemID, req.amount);
        }
    }

    public bool GiveOutput(ItemsSO item, int amount)
    {
        if (InventoryManager.Instance.InventoryFull())
        {
            Debug.Log("Cannot add crafted item: Inventory is full!");
            return false;
        }

        bool success = InventoryManager.Instance.AddItem(item, amount);
        if (success)
            Debug.Log($"Crafted {item.itemName} x{amount}");
        else
            Debug.Log("Failed to add crafted item to inventory!");

        return success;
    }
}
