using UnityEngine;
using System.Collections.Generic;

public class CrafterInteractV2 : MonoBehaviour, IInteractable
{
    public bool RequiresHold => false;
    [SerializeField] private CrafterManagerV2 crafterManager; 
    [SerializeField] private List<RecipeSO> stationRecipes;
    [SerializeField] private RectTransform progressContent;

    public void Interact()
    {
        if (CrafterUIManagerV2.Instance != null && CrafterUIManagerV2.Instance.gameObject.activeSelf)
        {
            CrafterUIManagerV2.Instance.Close();
        }
        else
        {
            if(CrafterUIManagerV2.Instance == null) Debug.LogWarning("CrafterUIManagerV2 Instance is null!");
            CrafterUIManagerV2.Instance.Open(crafterManager, stationRecipes, progressContent);
        }
    }
}
