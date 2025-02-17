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

    private Color _initialColor;

    private void Start()
    {
        _initialColor = GetComponentInChildren<SpriteRenderer>().color;
    }

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
            spriteRenderers.ForEach(i => i.color = _initialColor);
        }

        _currentTween = DOTween.To(
            () => _initialColor,
            x => ApplyColorSafely(x),
            flashColor,
            duration
        ).SetLoops(2, LoopType.Yoyo)
        .OnComplete(() => ApplyColorSafely(_initialColor));
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