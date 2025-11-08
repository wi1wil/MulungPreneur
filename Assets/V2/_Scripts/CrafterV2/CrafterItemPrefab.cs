using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrafterItemPrefab : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TMP_Text nameText;
    public Transform requirementsParent;
    public GameObject requirementPrefab;
    public Button craftButton;

    private RecipeSO recipe;
    private CrafterUIManagerV2 uiManager;

    public void Setup(RecipeSO r, CrafterUIManagerV2 manager)
    {
        recipe = r;
        uiManager = manager;

        icon.sprite = r.icon;
        nameText.text = r.recipeName;

        // Clear previous requirements
        foreach (Transform child in requirementsParent)
            Destroy(child.gameObject);

        // Get inventory data once
        var itemCounts = InventoryManager.Instance.GetItemCountBySO();

        bool canCraft = true;

        // Setup new requirements
        foreach (var req in r.inputs)
        {
            var go = Instantiate(requirementPrefab, requirementsParent);
            Sprite iconSprite = req.item.itemIcon;
            var ui = go.GetComponent<RecipeReqUI>();
            ui.Setup(iconSprite, req.item.name, req.amount);

            // Check inventory
            int currentAmount = itemCounts.ContainsKey(req.item) ? itemCounts[req.item] : 0;
            bool enough = currentAmount >= req.amount;
            ui.SetAmountColor(enough ? Color.green : Color.red);

            if (!enough) canCraft = false;
        }

        // Button interactable only if craftable
        craftButton.interactable = canCraft;

        // Setup craft button
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(OnCraftClicked);
    }

    private void OnCraftClicked()
    {
        uiManager.OnItemCraftClicked(recipe);
    }

    public void SetInteractable(bool v) => craftButton.interactable = v;
}
