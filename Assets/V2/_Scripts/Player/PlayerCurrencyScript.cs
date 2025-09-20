using UnityEngine;
using TMPro;

public class PlayerCurrencyScript : MonoBehaviour
{
    public int playersCurrency = 0;
    public TMP_Text currencyText;

    public void AddCurrency(int itemValue)
    {
        playersCurrency += itemValue;
        updateText();
    }

    public void updateText()
    {
        currencyText.text = playersCurrency.ToString();
    }
}
