using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CrafterUIManager : MonoBehaviour
{
     [Header("References")]
    [SerializeField] private CrafterManager crafterManager;

    [Header("Recipe List UI")]
    public RectTransform content; 
    public GameObject craftItemPrefab;
    public List<RecipesSO> recipes;

    [Header("Crafting Progress UI")]
    public RectTransform progressContent;
    public GameObject craftingSlotPrefab;

    private List<CrafterItemPrefabs> spawned = new List<CrafterItemPrefabs>();

    void Start() => Populate();

    void Populate()
    {
        foreach (var r in recipes)
        {
            var go = Instantiate(craftItemPrefab, content);
            var ui = go.GetComponent<CrafterItemPrefabs>();
            ui.Setup(r, this);
            spawned.Add(ui);
        }
    }

    public void SetAllButtonsInteractable(bool v)
    {
        foreach (var ui in spawned) ui.SetInteractable(v);
    }

    public void OnItemCraftClicked(RecipesSO recipe)
    {
        crafterManager.TryCraft(recipe);
    }

    public CrafterSlotUI AddCraftingSlot(RecipesSO recipe)
    {
        var go = Instantiate(craftingSlotPrefab, progressContent);
        var slot = go.GetComponent<CrafterSlotUI>();
        slot.Setup(recipe, crafterManager);
        return slot;
    }
}
