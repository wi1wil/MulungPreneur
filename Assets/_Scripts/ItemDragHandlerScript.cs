using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandlerScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDis = 2f;
    public float maxDropDis = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Follow the mouse position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot == null)
        {
            GameObject item = eventData.pointerEnter;
            if (item != null)
            {
                dropSlot = item.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // if dropping is not inside the inventory, drop the item
            if (!isWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
            }
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool isWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDis, maxDropDis);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;
        Instantiate(gameObject, dropPosition, Quaternion.identity);
        Destroy(gameObject);
    }

}
