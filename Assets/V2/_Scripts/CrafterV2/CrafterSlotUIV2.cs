using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrafterSlotUIV2 : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text craftingNameText;
    public Image progressFill;
    public Button claimButton;
    public Image itemIcon;

    private RecipeSO recipe;
    private CrafterManagerV2 crafter;

    public void Setup(RecipeSO r, CrafterManagerV2 manager)
    {
        recipe = r;
        crafter = manager;

        ItemsSO outputItem = recipe.outputItem;

        craftingNameText.text = "Currently Crafting: " + recipe.recipeName;

        if (itemIcon != null && outputItem != null && outputItem.itemIcon != null)
            itemIcon.sprite = outputItem.itemIcon;

        progressFill.fillAmount = 0f;
        claimButton.gameObject.SetActive(false);

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClaimClicked);
    }

    public void UpdateProgress(float t)
    {
        progressFill.fillAmount = t;
    }

    public void MarkAsFinished()
    {
        progressFill.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(true);
        craftingNameText.text = recipe.recipeName + " ready!";
    }

    private void OnClaimClicked()
    {
        bool success = crafter.GiveOutput(recipe.outputItem, recipe.outputAmount);

        if (success)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full – cannot claim!");
        }
    }
}
