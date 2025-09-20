using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private Image holdProgressBar;

    private List<IInteractable> interactablesInRange = new List<IInteractable>();
    private IInteractable currentTarget;

    public float holdDuration = 1f;
    private float holdTimer = 0f;
    private bool isHolding = false;

    void Start()
    {
        interactionIcon.SetActive(false);
        holdProgressBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isHolding && currentTarget != null)
        {
            holdTimer += Time.deltaTime;
            holdProgressBar.fillAmount = holdTimer / holdDuration;

            if (holdTimer >= holdDuration)
            {
                currentTarget.Interact();
                isHolding = false;
                holdTimer = 0f;
                holdProgressBar.gameObject.SetActive(false);
                holdProgressBar.fillAmount = 0f;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentTarget != null)
            {
                isHolding = true;
                holdTimer = 0f;
                holdProgressBar.gameObject.SetActive(true);
            }
        }
        else if (context.canceled)
        {
            isHolding = false;
            holdTimer = 0f;
            holdProgressBar.gameObject.SetActive(false);
            holdProgressBar.fillAmount = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactablesInRange.Add(interactable);
            currentTarget = GetClosestInteractable();
            interactionIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactablesInRange.Remove(interactable);
            currentTarget = GetClosestInteractable();

            if (currentTarget == null)
                interactionIcon.SetActive(false);
        }
    }

    private IInteractable GetClosestInteractable()
    {
        if (interactablesInRange.Count == 0) return null;

        float minDist = float.MaxValue;
        IInteractable closest = null;

        foreach (var i in interactablesInRange)
        {
            var go = (i as MonoBehaviour).gameObject;
            float dist = Vector2.Distance(transform.position, go.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = i;
            }
        }
        return closest;
    }
}