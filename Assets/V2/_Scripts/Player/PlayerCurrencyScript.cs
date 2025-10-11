using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCurrencyScript : MonoBehaviour
{
    public int playerCurrency;
    public Text currencyText;

    public void AddCurrency(int itemValue)
    {
        playerCurrency += itemValue;
        UpdateText();
    }

    public void DecreaseCurrency(int amount)
    {
        playerCurrency -= amount;
        UpdateText();
    }

    public void UpdateText()
    {
        currencyText.text = playerCurrency.ToString();
    }
}
