using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHelper : MonoBehaviour
{
    private GameObject targetObject;

    private void Awake()
    {
        if (transform.parent != null)
            targetObject = transform.parent.gameObject;
        else
            targetObject = gameObject; 
    }

    public void DestroyMe()
    {
        if (targetObject != null)
            Destroy(targetObject);
    }
}
