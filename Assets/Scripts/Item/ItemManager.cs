using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orby.Core.Singleton;
using TMPro;
using UnityEngine.Events;
using Unity.Collections;

public class ItemManager : Singleton<ItemManager>
{
    //public IntData coins;
    //public IntData rocks;
    public AudioClip coinPickUpSound;
    public UnityEvent onReset;
    public UnityEvent onAddCoin;
    public UnityEvent onAddRock;

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        //coins.value = 0;
        //rocks.value = 0;
        onReset?.Invoke();
    }

    public void AddCoins(int amount = 1)
    {
        //coins.value += amount;
        onAddCoin?.Invoke();
    }

    public void AddRocks(int amount = 1)
    {
        //rocks.value += amount;
        onAddRock?.Invoke();
    }
}
