using System;
using System.Collections;
using System.Collections.Generic;
using Orby.Enemy.Boss.Phases.FirstPhase;
using Orby.Interfaces;
using Orby.Interfaces.General;
using Orby.Managers;
using UnityEngine;

namespace Orby.Enemy
{
    public class EnemyShooter : EnemyBase
    {
        [Header("Fireball")]
        [SerializeField] private Fireball fireballPrefab;
        [SerializeField] private float fireballSpeed;

        [Header("Shoot point")]
        [SerializeField] private Transform shootPoint;

        [Header("Limitations")]
        [SerializeField] private float shootSpeed = 5;

        public BoxCollider2D _collider;

        // Event
        public static event Action<EnemyShooter> OnShooterKilled;

        private Vector3 _fireballDirection;
        private bool _shooting = true;

        protected override void Start()
        {
            base.Start();

            float yRotation = transform.eulerAngles.y;

            _fireballDirection.x = (Mathf.Approximately(yRotation, 180f)) ? 1 : -1;
        }

        public void WillCollide(bool willCollide)
        {
            _collider.enabled = willCollide;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameOver == true)
                _shooting = false;
        }

        public IEnumerator Shoot()
        {
            while (_shooting)
            {
                Animator.SetTrigger("Attack");
                // Animation attack time
                yield return new WaitForSeconds(0.25f);

                AudioManager.Instance.PlayAudioByTypeWithRandomPitch(
                    AudioManager.AudioType.SHOOTERATTACK,
                    new Vector2(0.7f, 1.3f),
                    0.3f
                );

                var fireball = Instantiate(fireballPrefab);
                fireball.transform.position = shootPoint.position;
                fireball.Direction = _fireballDirection * fireballSpeed;

                WillCollide(true);

                yield return new WaitForSeconds(0.4f);
                Animator.SetTrigger("Idle");
                yield return new WaitForSeconds(shootSpeed);
            }
        }

        public override void Kill()
        {
            base.Kill();

            _shooting = false;
            StopCoroutine(Shoot());

            OnShooterKilled?.Invoke(this);

            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                var damageable = collision.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(1);
                }
            }
        }
    }
}
