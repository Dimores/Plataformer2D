using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using TMPro;

public class ItemManager : Singleton<ItemManager>
{
    public int coins;

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        coins = 0;
        UpdateCoinUI();
    }

    public void AddCoins(int amount = 1)
    {
        coins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        UIInGameManager.UpdateTextCoins(coins.ToString());
    }
}
