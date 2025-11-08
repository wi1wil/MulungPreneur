using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeReqUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text text;

    public void SetAmountColor(Color color)
    {
        text.color = color; 
    }

    public void Setup(Sprite sprite, string itemName, int amount)
    {
        icon.sprite = sprite;
        text.text = $"x{amount}";
    }
}
