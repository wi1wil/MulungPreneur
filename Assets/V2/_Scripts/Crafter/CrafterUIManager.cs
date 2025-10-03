using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CrafterUIManager : MonoBehaviour
{
    public static CrafterUIManager Instance;

    [Header("References")]
    private CrafterManager activeCrafter;
    private RectTransform activeProgressPanel; 

    [Header("Recipe List UI")]
    public RectTransform content; 
    public GameObject craftItemPrefab;

    [Header("Crafting Slot Prefab")]
    public GameObject craftingSlotPrefab;

    private List<ItemRecipePrefabs> spawned = new List<ItemRecipePrefabs>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameObject.SetActive(false);
    }

    public void Open(CrafterManager crafter, List<RecipesSO> recipes, RectTransform progressPanel)
    {
        activeCrafter = crafter;
        activeProgressPanel = progressPanel;

        if (activeProgressPanel != null)
            activeProgressPanel.gameObject.SetActive(true);

        gameObject.SetActive(true);

        foreach (var ui in spawned) Destroy(ui.gameObject);
        spawned.Clear();

        foreach (var r in recipes)
        {
            var go = Instantiate(craftItemPrefab, content);
            var ui = go.GetComponent<ItemRecipePrefabs>();
            ui.Setup(r, this);
            spawned.Add(ui);
        }
    }

    public void Close()
    {
        if (activeProgressPanel != null)
            activeProgressPanel.gameObject.SetActive(false);

        activeCrafter = null;
        activeProgressPanel = null;
        gameObject.SetActive(false);
    }

    public void OnItemCraftClicked(RecipesSO recipe)
    {
        if (activeCrafter != null)
            activeCrafter.TryCraft(recipe);
    }

    public CrafterSlotUI AddCraftingSlot(RecipesSO recipe, CrafterManager crafter)
    {
        if (activeProgressPanel == null)
        {
            Debug.LogWarning("No active progress panel set!");
            return null;
        }

        var go = Instantiate(craftingSlotPrefab, activeProgressPanel);
        var slot = go.GetComponent<CrafterSlotUI>();
        slot.Setup(recipe, crafter);
        return slot;
    }
}
