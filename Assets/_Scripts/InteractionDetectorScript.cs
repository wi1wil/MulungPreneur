using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetectorScript : MonoBehaviour
{
    private IInteractable InteractableInRange = null;
    private ShopKeeperScript ShopKeeperInRange = null; // Track shopkeeper separately
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractableInRange?.Interact();
        }
    }

    // New method for shop interaction (bind this to V in your Input Actions)
    public void OnShopInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShopKeeperInRange?.OpenShop();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.canInteract())
        {
            InteractableInRange = interactable;
            interactionIcon.SetActive(true);
        }
        // Detect shopkeeper
        if (collision.TryGetComponent(out ShopKeeperScript shopKeeper))
        {
            ShopKeeperInRange = shopKeeper;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == InteractableInRange)
        {
            InteractableInRange = null;
            interactionIcon.SetActive(false);
        }
        if (collision.TryGetComponent(out ShopKeeperScript shopKeeper) && shopKeeper == ShopKeeperInRange)
        {
            ShopKeeperInRange = null;
        }
    }
}