using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _quantityText;
    private ItemsSO _currentItem;
    private int _quantity;

    public Image IconImage => _iconImage;
    public ItemStack CurrentStack { get; private set; }
    public int SlotIndex { get; private set; }

    public void SetItem(ItemStack stack)
    {
        CurrentStack = stack;

        if (stack == null || stack.item == null || stack.IsEmpty())
        {
            ClearSlot();
            return;
        }

        _iconImage.sprite = stack.item.itemIcon;
        _iconImage.enabled = true;
        _quantityText.text = stack.quantity > 1 ? stack.quantity.ToString() : "";
    }

    public void UpdateQuantity()
    {
        if (CurrentStack == null || CurrentStack.IsEmpty())
        {
            ClearSlot();
            return;
        }

        _quantityText.text = CurrentStack.quantity > 1 ? CurrentStack.quantity.ToString() : "";
    }

    public void ClearSlot()
    {
        CurrentStack = null;
        _iconImage.sprite = null;
        _iconImage.enabled = false;
        _quantityText.text = "";
    }

    public void SetIndex(int index)
    {
        SlotIndex = index;
    }

    public Sprite GetIcon()
    {
        return _iconImage != null ? _iconImage.sprite : null;
    }
}
