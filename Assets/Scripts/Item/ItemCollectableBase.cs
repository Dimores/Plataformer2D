using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableBase : MonoBehaviour
{
    [Header("Sprite")]
    public GameObject sprite;

    [Header("Collision tag")]
    public string compareTag = "Player";

    [Header("Sounds")]
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(compareTag))
        {
            Collect();
        }    
    }

    protected virtual void Collect() {
        if (sprite != null) sprite.SetActive(false);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        Invoke("HideObject", 3);
        OnCollect();
    }



    private void HideObject()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnCollect() {
        if (audioSource != null) audioSource.Play();
        
    }
}
