using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public Action OnKill;

    public int startLife = 10;

    public bool destroyOnKill = false;

    public float delayToKill = 0f;

    private int _currentLife;
    private bool _isDead = false;

    private FlashColor _flashColor;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _isDead = false;
        _currentLife = startLife;

        _flashColor = GetComponentInChildren<FlashColor>(true);
    }


    public void Damage(int damage)
    {
        if (_isDead) return;

        _currentLife -= damage;

        if (_flashColor != null)
            _flashColor.Flash();

        if (_currentLife <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        _isDead = true;

        if (destroyOnKill)
        {
            Destroy(gameObject, delayToKill);
        }
        OnKill?.Invoke();
    }
}