using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ItemRecipePrefabs : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TMP_Text nameText;
    public Transform requirementsParent;
    public GameObject requirementPrefab;
    public Button craftButton;

    private RecipesSO recipe;
    private CrafterUIManager uiManager;

    public void Setup(RecipesSO r, CrafterUIManager manager)
    {
        recipe = r;
        uiManager = manager;

        icon.sprite = r.icon;
        nameText.text = r.recipeName;

        foreach (Transform child in requirementsParent)
            Destroy(child.gameObject);

        foreach (var req in r.inputs)
        {
            var go = Instantiate(requirementPrefab, requirementsParent);

            GameObject itemPrefab = ItemDictionaryScript.Instance.GetItemPrefab(req.item.id);
            Sprite iconSprite = null;
            if (itemPrefab != null)
            {
                var sr = itemPrefab.GetComponent<SpriteRenderer>();
                if (sr != null)
                    iconSprite = sr.sprite;
                else
                {
                    var img = itemPrefab.GetComponent<Image>();
                    if (img != null)
                        iconSprite = img.sprite;
                }
            }

            var ui = go.GetComponent<RecipeRequirementsUI>();
            ui.Setup(iconSprite, req.item.name, req.amount);
        }

        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(OnCraftClicked);
    }

    private void OnCraftClicked()
    {
        uiManager.OnItemCraftClicked(recipe);
    }

    public void SetInteractable(bool v) => craftButton.interactable = v;
}