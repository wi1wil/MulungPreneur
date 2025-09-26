using UnityEngine;

public class CrafterInteract : MonoBehaviour, IInteractables
{
    public GameObject craftingUI;
    
    public void Interact()
    {
        Debug.Log("Interacting with Crafter");
        openUI();
    }

    public bool canInteract()
    {
        Debug.Log("You can interact with the Crafter");
        return true;
    }

    public void openUI()
    {
        if (craftingUI.activeSelf)
        {
            craftingUI.SetActive(false);
        }
        else
        {
            craftingUI.SetActive(true);
        }
    }
}
