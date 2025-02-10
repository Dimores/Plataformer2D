using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHelper : MonoBehaviour
{
    public GameObject targetObject;
    public void DestroyMe()
    {
        if (targetObject != null)
            Destroy(targetObject);
    }
}
