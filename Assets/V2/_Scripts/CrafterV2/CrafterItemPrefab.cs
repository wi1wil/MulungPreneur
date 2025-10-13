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

        // Setup new requirements
        foreach (var req in r.inputs)
        {
            var go = Instantiate(requirementPrefab, requirementsParent);

            // Get icon directly from ItemsSO
            Sprite iconSprite = req.item.itemIcon;

            var ui = go.GetComponent<RecipeRequirementsUI>();
            ui.Setup(iconSprite, req.item.name, req.amount);
        }

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
