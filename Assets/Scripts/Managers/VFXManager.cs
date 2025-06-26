using System.Collections;
using System.Collections.Generic;
using Orby.Core.Singleton;
using UnityEngine;

namespace Orby.Managers
{
    public class VFXManager : Singleton<VFXManager>
    {
        public enum VFXType
        {
            WALK,
            JUMP,
            FALL,
            COIN,
            EXPLOSION,
            EXPLOSIONPLAYER,
            EXPLOSIONLASER,
            DASH
        }

        public List<VFXManagerSetup> vfxSetup;

        #region PlayVFXByType
        public void PlayVFXByType(VFXType vfxType, Vector3 position,
            bool willDestroy = true)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, null);
                    item.transform.position = position;
                    if (willDestroy)
                        Destroy(item.gameObject, 3f);
                    break;
                }
            }
        }

        public void PlayVFXByType(VFXType vfxType, Vector3 position, Vector3 offset,
            bool willDestroy = true)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, null);
                    item.transform.position = position + offset;
                    if (willDestroy)
                        Destroy(item.gameObject, 3f);
                    break;
                }
            }
        }

        public void PlayVFXByType(VFXType vfxType, Vector3 position, Transform parent,
            bool willDestroy = true)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, null);
                    item.transform.position = position;
                    if (willDestroy)
                        Destroy(item.gameObject, 3f);
                    break;
                }
            }
        }

        public void PlayVFXByType(VFXType vfxType, Vector3 position, Vector3 offset,
            Transform parent, bool willDestroy = true)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, parent);
                    item.transform.position = position + offset;
                    if (willDestroy)
                        Destroy(item.gameObject, 3f);
                    break;
                }
            }
        }
        #endregion

        #region PlayVFBByTypeWithCollision
        public void PlayVFXByTypeWithCollision(VFXType vfxType, Vector3 position, Transform parent,
            Transform planeTransform, bool willDestroy = true)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, null);
                    item.transform.position = position;

                    var particleSystem = item.GetComponent<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        var collisionModule = particleSystem.collision;
                        collisionModule.SetPlane(0, planeTransform);
                    }

                    if (willDestroy)
                        Destroy(item.gameObject, 3f);
                    break;
                }
            }
        }
        #endregion

        #region PlayPermanentVFX
        public ParticleSystem PlayAndGetPermanentVFXByType(VFXType vfxType, Vector3 position, Vector3 offset,
            Transform parent)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, parent);
                    item.transform.position = position + offset;

                    var particleSystem = item.GetComponent<ParticleSystem>();
                    return particleSystem;
                }
            }
            return null;
        }

        public void PlayPermanentVFXByType(VFXType vfxType, Vector3 position, Vector3 offset,
            Transform parent)
        {
            foreach (var vfx in vfxSetup)
            {
                if (vfx.vfxType == vfxType)
                {
                    var item = Instantiate(vfx.prefab, parent);
                    item.transform.position = position + offset;
                }
            }
        }
        #endregion
    }

    [System.Serializable]
    public class VFXManagerSetup
    {
        public VFXManager.VFXType vfxType;
        public GameObject prefab;
    }
}
