using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableRock : ItemCollectableBase
{
    protected override void OnCollect()
    {
        ItemManager.Instance.AddRocks(5);

        Destroy(gameObject);
    }
}

