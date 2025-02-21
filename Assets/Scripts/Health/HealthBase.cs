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

    public FlashColor _flashColor;

    public bool IsDead { get => _isDead; set => _isDead = value; }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        IsDead = false;
        _currentLife = startLife;

        _flashColor = GetComponentInChildren<FlashColor>(true);

        if (_flashColor == null)
            Debug.LogError($"FlashColor não encontrado em {gameObject.name}");
    }

    public void Damage(int damage)
    {
        if (IsDead) return;

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