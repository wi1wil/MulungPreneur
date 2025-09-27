using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class CrafterSlotUI : MonoBehaviour
{
    public TMP_Text craftingNameText;
    public Image progressFill;
    public Button claimButton;
    public Image itemIcon;

    private RecipesSO recipe;

    public void Setup(RecipesSO r)
    {
        recipe = r;
        craftingNameText.text = "Currently Crafting: " + r.recipeName;
        if (itemIcon != null && r.icon != null)
            itemIcon.sprite = r.icon;

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
        bool success = CrafterManager.Instance.GiveOutput(recipe);
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
