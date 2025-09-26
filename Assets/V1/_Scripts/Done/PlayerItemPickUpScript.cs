using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerItemPickUpScript : MonoBehaviour
{
    private InventoryManagerScripts inventoryManager;
    private AudioManagerScript audioScript;
    private float holdTimer = 0f;
    public Image fillCirclePrefab;
    private Image currentFillCircle;
    public bool isHolding = false;
    public float holdDuration = 1f;
    public float pickUpRange = 1f;
    private GameObject nearestItem;
    private Camera mainCamera;
    public Canvas mainCanvas;

    private SaveDataScript saveData;

    bool sfxPlayed = false;

    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManagerScripts>();
        audioScript = FindFirstObjectByType<AudioManagerScript>();
        
        if (!audioScript)
            audioScript = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();

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

            if (!sfxPlayed)
                TrashSFX();

            holdTimer += Time.deltaTime;
            currentFillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                Item item = nearestItem.GetComponent<Item>();
                bool itemAdded = inventoryManager.AddItem(nearestItem);
                if (itemAdded)
                {
                    item.ShowPopup();
                    Destroy(nearestItem);
                    ResetHold();
                }
            }
        }
        else
        {
            sfxPlayed = false;
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

    public void updatePickUpRange(float additionRange)
    {
        pickUpRange += additionRange;
    }

    public void updatePickUpSpeed(float additionSpeed)
    {
        holdDuration -= additionSpeed;
        Debug.Log("Updating hold duration: " + holdDuration);
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

    void TrashSFX()
    {
        audioScript.PlaySfx(audioScript.pickUpTrash);
        sfxPlayed = true;
    }
}
