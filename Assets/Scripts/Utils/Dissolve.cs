using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Utils
{
    public class Dissolve : MonoBehaviour
    {
        [SerializeField] private float _dissolveTime = 1f;

        private SpriteRenderer[] _spriteRenderers;
        private Material[] _materials;

        private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

        public SpriteRenderer[] SpriteRenderers { get => _spriteRenderers; set => _spriteRenderers = value; }

        private void Start()
        {
            SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            _materials = new Material[SpriteRenderers.Length];
            for (int i = 0; i < SpriteRenderers.Length; i++)
            {
                _materials[i] = SpriteRenderers[i].material;
            }
        }

        public IEnumerator Vanish()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _dissolveTime)
            {
                elapsedTime += Time.deltaTime;

                float lerpedDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / _dissolveTime));

                for (int i = 0; i < _materials.Length; i++)
                {
                    _materials[i].SetFloat(_dissolveAmount, lerpedDissolve);
                }

                yield return null;
            }
            
        }
    }
}
