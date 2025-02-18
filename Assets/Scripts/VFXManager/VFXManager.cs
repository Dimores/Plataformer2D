using System.Collections;
using System.Collections.Generic;
using Ebac.Core.Singleton;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    public enum VFXType
    {
        JUMP,
        FALL
    }

    public List<VFXManagerSetup> vfxSetup;

    public void PlayVFXByType(VFXType vfxType, Vector3 position)
    {
        foreach (var vfx in vfxSetup)
        {
            if (vfx.vfxType == vfxType)
            {
                var item = Instantiate(vfx.prefab);
                item.transform.position = position;
                Destroy(item.gameObject, 3f);
                break;
            }
        }

    }
}

[System.Serializable]
public class VFXManagerSetup
{
    public VFXManager.VFXType vfxType;
    public GameObject prefab;
}
