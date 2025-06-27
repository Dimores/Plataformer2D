using UnityEngine;

namespace Orby.Utils
{
    public class MenuMovements : MonoBehaviour
    {
        [Header("Refs")]
        public Transform playerTransform;
        public RectTransform asteroidRectTransform;

        [Header("Movement config")]
        public float depth = 1f;
        public float speed = 1f;

        [Header("UI Scale Factor")]
        public float uiScaleFactor = 100f;

        private Vector3 basePositionTransform;
        private Vector3 basePositionRect;

        void Start()
        {
            if (playerTransform != null)
                basePositionTransform = playerTransform.position;

            if (asteroidRectTransform != null)
                basePositionRect = asteroidRectTransform.anchoredPosition;
        }

        void Update()
        {
            float offset = Mathf.Sin(Time.time * speed) * depth;

            if (playerTransform != null)
            {
                Vector3 novaPos = basePositionTransform + Vector3.up * offset;
                playerTransform.position = novaPos;
            }

            if (asteroidRectTransform != null)
            {
                Vector3 novaPos = basePositionRect + Vector3.up * offset * uiScaleFactor;
                asteroidRectTransform.anchoredPosition = novaPos;
            }
        }
    }
}
