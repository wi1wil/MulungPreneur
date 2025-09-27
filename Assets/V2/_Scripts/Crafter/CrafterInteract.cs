using UnityEngine;
using System.Collections.Generic;

public class CrafterInteract : MonoBehaviour, IInteractables
{
    [SerializeField] private CrafterManager crafterManager; 
    [SerializeField] private List<RecipesSO> stationRecipes;
    [SerializeField] private RectTransform progressContent;

    public void Interact()
    {
        if (CrafterUIManager.Instance != null && CrafterUIManager.Instance.gameObject.activeSelf)
        {
            CrafterUIManager.Instance.Close();
        }
        else
        {
            CrafterUIManager.Instance.Open(crafterManager, stationRecipes, progressContent);
        }
    }

    public bool canInteract()
    {
        Debug.Log("You can interact with the Crafter");
        return true;
    }
}
