using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWalletScript : MonoBehaviour
{
    public int playerMoney = 0;
    [SerializeField] Text walletText;

    private void Awake()
    {
        if(!walletText)
            walletText = GameObject.Find("Wallet-Text").GetComponent<Text>();
    }

    public void AddMoneyFromItemSold(int itemValue)
    {
        playerMoney += itemValue;
    }

    private void Update()
    {
        walletText.text = playerMoney.ToString();
    }
}
