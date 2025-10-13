using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CrafterUIManagerV2 : MonoBehaviour
{
    public static CrafterUIManagerV2 Instance;

    [Header("References")]
    private CrafterManagerV2 activeCrafter;
    private RectTransform activeProgressPanel; 

    [Header("Recipe List UI")]
    public RectTransform content; 
    public GameObject craftItemPrefab;

    [Header("Crafting Slot Prefab")]
    public GameObject craftingSlotPrefab;

    private List<CrafterItemPrefab> spawned = new List<CrafterItemPrefab>();

    void Awake()
    {
        if (Instance != null && Instance != this)
     {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(CrafterManagerV2 crafter, List<RecipeSO> recipes, RectTransform progressPanel)
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
            var ui = go.GetComponent<CrafterItemPrefab>();
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

    public void OnItemCraftClicked(RecipeSO recipe)
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
