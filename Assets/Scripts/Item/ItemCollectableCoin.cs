using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Importa o DOTween

public class ItemCollectableCoin : ItemCollectableBase
{
    [Header("Animation Config")]
    public float rotationSpeed = 1f; 
    public float floatHeight = 0.2f; 
    public float floatDuration = 1f;

    [Header("VFX Collider")]
    public Transform vfxCollider;

    private Tween _rotationTween;
    private Tween _floatTween;

    private void Start()
    {
        AnimateCoin(); 
    }

    private void AnimateCoin()
    {
        _floatTween = transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddCoins();
        VFXManager.Instance.PlayVFXByTypeWithCollision(VFXManager.VFXType.COIN, this.transform.position, 
            null, vfxCollider);
    }

    private void OnDestroy()
    {
        _floatTween?.Kill();
    }
}
