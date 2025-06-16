using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalletScript : MonoBehaviour
{
    public int playerMoney = 0;

    public void AddMoneyFromItemSold(int itemValue)
    {
        playerMoney += itemValue;
    }
}
