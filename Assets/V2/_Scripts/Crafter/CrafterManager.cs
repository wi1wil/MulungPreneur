using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CrafterManager : MonoBehaviour
{
    public static CrafterManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryCraft(RecipesSO recipe)
    {
        if (!HasEnoughItems(recipe))
        {
            Debug.Log("Not enough resources to craft " + recipe.recipeName);
            return false;
        }

        RemoveInputs(recipe);
        StartCoroutine(CraftRoutine(recipe));
        return true;
    }

    private IEnumerator CraftRoutine(RecipesSO recipe)
    {
        var slot = CrafterUIManager.Instance.AddCraftingSlot(recipe);

        float timer = 0f;
        while (timer < recipe.craftTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / recipe.craftTime);

            slot.UpdateProgress(progress);

            yield return null;
        }

        slot.MarkAsFinished();
    }

    private bool HasEnoughItems(RecipesSO recipe)
    {
        var counts = InventoryManagerScripts.Instance.GetItemCount();
        foreach (var req in recipe.inputs)
        {
            if (!counts.ContainsKey(req.item.id) || counts[req.item.id] < req.amount)
                return false;
        }
        return true;
    }

    private void RemoveInputs(RecipesSO recipe)
    {
        foreach (var req in recipe.inputs)
        {
            InventoryManagerScripts.Instance.RemoveItemsFromInv(req.item.id, req.amount);
        }
    }

    public bool GiveOutput(RecipesSO recipe)
    {
        if (InventoryManagerScripts.Instance.IsInventoryFull())
        {
            Debug.Log("Cannot add crafted item: Inventory is full!");
            return false;
        }

        var prefab = recipe.outputPrefab;
        if (prefab != null)
        {
            GameObject clone = Instantiate(prefab);
            var item = clone.GetComponent<Item>();
            item.Quantity = recipe.outputAmount;
            item.UpdateQuantity();

            InventoryManagerScripts.Instance.AddItem(clone);
            Destroy(clone); 
        }

        return true;
    }   
}
