using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetectorScript : MonoBehaviour
{
    private IInteractable InteractableInRange = null;
    private ShopKeeperScript ShopKeeperInRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnShopInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ShopKeeperInRange != null)
            {
                ShopKeeperInRange?.OpenShop();
                Debug.Log("Shop interaction triggered");
            }
        }
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractableInRange?.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ShopKeeperScript shopKeeper))
        {
            Debug.Log("Entered shopkeeper range");
            ShopKeeperInRange = shopKeeper;
        }

        if (collision.TryGetComponent(out IInteractable interactable) && interactable.canInteract())
        {
            InteractableInRange = interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ShopKeeperScript shopKeeper) && shopKeeper == ShopKeeperInRange)
        {
            Debug.Log("Exited shopkeeper range");
            ShopKeeperInRange = null;
        }

        if (collision.TryGetComponent(out IInteractable interactable) && interactable == InteractableInRange)
        {
            InteractableInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}
