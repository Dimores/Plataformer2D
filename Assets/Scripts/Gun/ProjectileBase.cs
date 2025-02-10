using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private Vector3 direction;

    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
