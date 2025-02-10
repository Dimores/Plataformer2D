using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlashColor : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenderers;
    public Color flashColor = Color.red;
    public float duration = .3f;

    public Tween _currentTween;

    private void OnValidate()
    {
        spriteRenderers = new List<SpriteRenderer>();
        foreach (var child in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderers.Add(child);
        }
    }

    public void Flash()
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
            spriteRenderers.ForEach(i => i.color = Color.white);
        }

        _currentTween = DOTween.To(
            () => Color.white,
            x => ApplyColorSafely(x),
            flashColor,
            duration
        ).SetLoops(2, LoopType.Yoyo)
        .OnComplete(() => ApplyColorSafely(Color.white));
    }


    private void ApplyColorSafely(Color color)
    {
        if (this == null || gameObject == null) return; 

        foreach (var s in spriteRenderers)
        {
            if (s != null) s.color = color;
        }
    }

    private void OnDestroy()
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
    }

}