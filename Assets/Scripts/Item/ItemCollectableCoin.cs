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

    private Tween _rotationTween;
    private Tween _floatTween;

    private void Start()
    {
        AnimateCoin(); 
    }

    private void AnimateCoin()
    {
        _rotationTween = transform.DORotate(new Vector3(0, 360, 0), rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear); 

        _floatTween = transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddCoins();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _rotationTween?.Kill();
        _floatTween?.Kill();
    }
}
