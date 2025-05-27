using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerItemPickUpScript : MonoBehaviour
{
    private InventoryManagerScript inventoryManager;
    public float holdDuration = 1f;
    private float holdTimer = 0f;
    public Image fillCirclePrefab;
    private Image currentFillCircle;
    public bool isHolding = false;
    public float pickUpRange = 1f;
    private GameObject nearestItem;
    public static event Action OnHoldComplete;
    private Camera mainCamera;
    public Canvas mainCanvas;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManagerScript>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        FindNearestItem();
        if (nearestItem != null && isHolding)
        {
            if (currentFillCircle == null)
            {
                currentFillCircle = Instantiate(fillCirclePrefab, mainCanvas.transform);
            }
            else
            {
                Vector3 worldPos = nearestItem.transform.position + Vector3.up * 0.75f;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
                currentFillCircle.transform.position = screenPos;
            }
            holdTimer += Time.deltaTime;
            currentFillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                Item item = nearestItem.GetComponent<Item>();
                bool itemAdded = inventoryManager.AddItem(nearestItem);
                if(itemAdded)
                {
                    item.PickUp();
                    Destroy(nearestItem);
                    ResetHold();
                }
            }
        }
    }

    void FindNearestItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickUpRange);
        nearestItem = null;
        float minDistance = float.MaxValue;
        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDistance && hit.CompareTag("Item"))
            {
                minDistance = dist;
                nearestItem = hit.gameObject;
                Debug.Log("Nearest item: " + nearestItem);
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true;
        }
        else if (context.canceled)
        {
            ResetHold();
        }
    }

    void ResetHold()
    {
        isHolding = false;
        if (currentFillCircle != null)
        {
            Destroy(currentFillCircle.gameObject);
            currentFillCircle = null;
        }
        holdTimer = 0f;
    }

    void OnDrawGizmosSelected()
   {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }
}
