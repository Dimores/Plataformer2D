using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Orby.Managers;
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
    }

    public void Damage(int damage)
    {
        if (IsDead) return;

        _currentLife -= damage;
        AudioManager.Instance.PlayAudioByTypeWithRandomPitch(AudioManager.AudioType.SLIMEDAMAGE, 
            new Vector2(0.8f, 1.2f) , 0.4f);

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