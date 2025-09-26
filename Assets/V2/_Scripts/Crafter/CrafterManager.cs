using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CrafterManager : MonoBehaviour
{
    private bool isCrafting = false;
    public static CrafterManager Instance;

    public bool TryCraft(RecipesSO recipe)
    {
        if (isCrafting)
        {
            Debug.Log("Already crafting something!");
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

    private IEnumerator CraftRoutine(RecipesSO recipe)
    {
        isCrafting = true;

        float timer = 0f;
        while (timer < recipe.craftTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / recipe.craftTime);

            CrafterUIManager.Instance.UpdateCraftingProgress(progress);

            yield return null;
        }
        
        GiveOutput(recipe);
        Debug.Log(recipe.recipeName + " crafted!");
        CrafterUIManager.Instance.OnCraftingFinished();
        isCrafting = false;
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

    private void GiveOutput(RecipesSO recipe)
    {
        if (InventoryManagerScripts.Instance.IsInventoryFull())
        {
            Debug.Log("Cannot add crafted item: Inventory is full!");
            return;
        }

        var prefab = recipe.outputPrefab;
        if (prefab != null)
        {
            GameObject clone = Instantiate(prefab);
            var item = clone.GetComponent<Item>();
            item.Quantity = recipe.outputAmount;
            item.UpdateQuantity();

            InventoryManagerScripts.Instance.AddItem(clone);
        }
    }
}
