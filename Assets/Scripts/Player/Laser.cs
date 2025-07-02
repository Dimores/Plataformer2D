using UnityEngine;
using DG.Tweening;
using Orby.Managers;
using System.Collections;
using Orby.Enemy;

namespace Orby.Player
{
    public class Laser : MonoBehaviour
    {
        [Header("Laser configs")]
        public float goTime = 1f;
        public float backTime = 0.5f;
        public float maxAlpha = 12f;
        public float timeToStopParticles = 1f;

        private ParticleSystem particles;
        private LineRenderer lineRenderer;
        private Material lineMaterial;
        private EdgeCollider2D edgeCollider;
        private Vector2 laserStart;
        private Vector2 laserEnd;

        public void Initialize(Vector3 start, Vector3 end)
        {
            laserStart = start;
            laserEnd = end;

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            lineMaterial = lineRenderer.material = new Material(lineRenderer.material);
            lineMaterial.SetFloat("_Alpha", 0f);

            particles = GetComponentInChildren<ParticleSystem>();

            if (particles != null)
            {
                Vector3 direction = end - start;
                float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (end.x < start.x)
                    zAngle += 180f;

                Quaternion laserRotation = Quaternion.Euler(0f, 0f, zAngle);
                Vector3 laserMid = (start + end) * 0.5f;

                Transform particleTransform = particles.transform;
                particleTransform.position = laserMid;
                particleTransform.rotation = laserRotation;

                var shape = particles.shape;
                shape.scale = new Vector3(Vector3.Distance(start, end), shape.scale.y, shape.scale.z);

                var main = particles.main;
                main.startRotation = 0f;
            }

            edgeCollider = GetComponent<EdgeCollider2D>();
            if (edgeCollider != null)
            {
                Vector2 localStart = transform.InverseTransformPoint(start);
                Vector2 localEnd = transform.InverseTransformPoint(end);
                edgeCollider.points = new Vector2[] { localStart, localEnd };
            }


            StartAlphaAnimation();
        }

        private void StartAlphaAnimation()
        {
            AudioManager.Instance.PlayAudioByType(AudioManager.AudioType.LOADLASER, 0.5f);

            DOTween.To(() => lineMaterial.GetFloat("_Alpha"),
                       valor => lineMaterial.SetFloat("_Alpha", valor),
                       maxAlpha, goTime)
                   .SetEase(Ease.InOutSine)
                   .OnComplete(() =>
                   {
                       edgeCollider.enabled = true;
                       AudioManager.Instance.PlayAudioByType(AudioManager.AudioType.SHOOTLASER, 1.2f);
                       StartCoroutine(StartParticles(timeToStopParticles));
                       DOTween.To(() => lineMaterial.GetFloat("_Alpha"),
                                  valor => lineMaterial.SetFloat("_Alpha", valor),
                                  0f, backTime)
                              .SetEase(Ease.InQuad);
                   });
        }

        private IEnumerator StartParticles(float timeToStop)
        {
            if (particles != null)
            {
                particles.Play();
                yield return new WaitForSeconds(timeToStop);
                particles.Stop();
            }

            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = collision.transform.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                Vector2 direction = (laserEnd - laserStart).normalized;
                float distance = Vector2.Distance(laserStart, laserEnd);

                RaycastHit2D hit = Physics2D.Raycast(laserStart, direction, distance, LayerMask.GetMask("Enemy"));
                Vector2 hitPoint = hit.collider != null ? hit.point : collision.ClosestPoint(laserStart);

                VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.EXPLOSIONLASER,
                    hitPoint,
                    null);
                AudioManager.Instance.PlayAudioByTypeWithRandomPitch(AudioManager.AudioType.SLIMEDAMAGE,
                    new Vector2(0.8f, 1.1f), 0.5f);
                enemy.Damage(20);
            }
        }
    }
}
