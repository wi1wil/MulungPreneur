using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventorySlot _slot;
    private Canvas _canvas;

    private Image _dragIcon;
    private RectTransform _dragIconRect;
    private static InventorySlot _originSlot;

    void Awake()
    {
        _slot = GetComponent<InventorySlot>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_slot.CurrentStack == null || _slot.CurrentStack.IsEmpty())
            return;

        _originSlot = _slot;

        if (_slot.IconImage != null)
        _slot.IconImage.color = new Color(1, 1, 1, 0.3f);

        _dragIcon = new GameObject("DragIcon").AddComponent<Image>();
        _dragIcon.transform.SetParent(_canvas.transform, false);
        _dragIcon.sprite = _slot.GetIcon();
        _dragIcon.SetNativeSize();

        _dragIconRect = _dragIcon.GetComponent<RectTransform>();
        _dragIconRect.sizeDelta *= 0.75f;
        _dragIconRect.pivot = new Vector2(0.5f, 0.5f);
        _dragIcon.raycastTarget = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragIconRect == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            _canvas.worldCamera,
            out var localPoint);

        _dragIconRect.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_slot.IconImage != null)
            _slot.IconImage.color = Color.white;
            
        if (_dragIcon != null)
            Destroy(_dragIcon.gameObject);

        var target = eventData.pointerEnter?.GetComponentInParent<InventorySlot>();
        if (target == null)
            return;

        InventoryManager.Instance.SwapOrStack(_originSlot, target);
    }
}
