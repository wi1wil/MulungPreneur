using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerCurrencyScript : MonoBehaviour
{
    public int playerCurrency;
    public List<Text> currencyText;

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
        for(int i = 0; i < currencyText.Count; i++)
        {
            currencyText[i].text = playerCurrency.ToString();
        }
    }
}
