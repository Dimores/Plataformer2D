using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using TMPro;
using UnityEngine.Events;

public class ItemManager : Singleton<ItemManager>
{
    public IntData coins;
    public AudioClip coinPickUpSound;
    public UnityEvent onAdd;

    private void Start()
    {
        Reset();
        SoundManager.Instance.setSound(coinPickUpSound);
    }

    private void Reset()
    {
        coins.value = 0;
    }

    public void AddCoins(int amount = 1)
    {
        coins.value += amount;
        onAdd?.Invoke();
    }
}
