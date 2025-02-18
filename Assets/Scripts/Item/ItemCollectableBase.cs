using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableBase : MonoBehaviour
{
    [Header("Collision tag")]
    public string compareTag = "Player";

    [Header("Particle System")]
    public ParticleSystem particles;

    private void Awake()
    {
        if (particles != null)
        {
            particles.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(compareTag))
        {
            Collect();
        }    
    }

    protected virtual void Collect() {
        gameObject.SetActive(false);
        OnCollect();
    }

    protected virtual void OnCollect() {
        if (particles != null)
        {
            particles.Play();
        }
    }
}
