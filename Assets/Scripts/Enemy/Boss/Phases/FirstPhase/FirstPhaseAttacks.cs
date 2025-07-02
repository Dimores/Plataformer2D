using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Orby.Interfaces.IPhase;
using Orby.Managers;
using UnityEngine;

namespace Orby.Enemy.Boss.Phases.FirstPhase
{
    public class FirstPhaseAttacks : MonoBehaviour, IPhase
    {
        [Header("Boss Data")]
        public Boss bossData;

        [Header("Limits")]
        private int consecutiveFireballLimit = 4;
        private int shooterLimit = 2;

        [Header("Shoot")]
        [SerializeField] private Fireball fireballPrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float timeBetweenShoots;
        [SerializeField] private float reloadTime;
        [SerializeField] private float fireballSpeed;

        [Header("Summon Shooter")]
        [SerializeField] private EnemyShooter shooterPrefab;

        [Header("VFX")]
        public GameObject portalPrefab;
        public Vector3 portalOffset;

        private int fireballCount = 0;
        private Vector3 playerDirection;
        private Coroutine currentFireballCoroutine;

        private int shooterAmount;

        public int ShooterAmount { get => shooterAmount; private set => shooterAmount = value; }

        public event Action OnAttackFinished;
        public event Action<Transform> OnSummonedEnemyKilled;


        #region INTERFACE
        public void FirstAttack()
        {
            if (currentFireballCoroutine == null)
                currentFireballCoroutine = StartCoroutine(FireballAttack());
        }

        public void SecondAttack(Transform summonPoint)
        {
            Vector3 rotation = Vector3.zero;

            if (summonPoint == GameManager.Instance.boss.SpawnPositions[0])
            {
                rotation = new Vector3(0f, 180f, 0f);
            }

            StartCoroutine(SummonShooter(summonPoint, rotation, shooterPrefab));
        }
        #endregion

        #region FIREBALL
        public void StartFireballAttack()
        {
            if(currentFireballCoroutine == null)
                currentFireballCoroutine = StartCoroutine(FireballAttack());
        }

        private IEnumerator FireballAttack()
        {
            fireballCount = 0;

            while (fireballCount < consecutiveFireballLimit)
            {
                bossData.Animator.SetTrigger("Fireball");

                yield return new WaitUntil(() => fireballCount >= consecutiveFireballLimit);
            }

            currentFireballCoroutine = null;
            bossData.Animator.SetTrigger("Idle");

            OnAttackFinished?.Invoke();
        }

        public void ShootFireball()
        {
            if (fireballCount >= consecutiveFireballLimit)
                return;

            Vector3 playerCenter = GameManager.Instance.Player.transform.position + new Vector3(0, 0.5f, 0);
            Vector3 rawDirection = playerCenter - shootPoint.position;
            rawDirection.z = 0f;
            playerDirection = rawDirection.normalized;

            var fireball = Instantiate(fireballPrefab);
            fireball.transform.position = shootPoint.position;
            fireball.Direction = playerDirection * fireballSpeed;

            AudioManager.Instance.PlayAudioByTypeWithRandomPitch(
                AudioManager.AudioType.FIREBALLATTACK,
                new Vector2(0.7f, 1f),
                0.2f
            );

            fireballCount++;
        }
        #endregion

        #region SHOOTER
        private IEnumerator SummonShooter(Transform summonPoint, Vector3 rotation, EnemyShooter shooter)
        {
            if (ShooterAmount >= shooterLimit)
                yield break;

            GameObject portal = Instantiate(portalPrefab, summonPoint.position + portalOffset, Quaternion.identity);
            Transform portalTransform = portal.transform;
            Vector3 originalScale = portalTransform.localScale;
            portalTransform.localScale = Vector3.zero;
            portal.transform.position += portalOffset;

            AudioManager.Instance.PlayAudioByTypeWithRandomPitch(
                AudioManager.AudioType.PORTALOPEN,
                new Vector2(1f, 1.1f),
                0.3f
                );
            portalTransform.DOScale(originalScale, 1.3f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(1.2f);

            EnemyShooter summonedShooter = Instantiate(shooter, summonPoint.position, Quaternion.Euler(rotation));
            summonedShooter.SpawnPosition = summonPoint;

            summonedShooter.WillCollide(false);

            List<SpriteRenderer> renderers = summonedShooter.SpriteRenderers;
            float fadeDuration = 2f;
            foreach (var sr in renderers)
            {
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
                sr.DOFade(1f, fadeDuration);
            }

            yield return new WaitForSeconds(fadeDuration);

            AudioManager.Instance.PlayAudioByTypeWithRandomPitch(
                AudioManager.AudioType.PORTALCLOSE,
                new Vector2(1f, 1.1f),
                0.3f
                );
            portalTransform.DOScale(Vector3.zero, 1.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    Destroy(portal); 
                });


            summonedShooter.StartCoroutine(summonedShooter.Shoot());

            ShooterAmount++;
        }

        public void SummonShooterWrapper(Transform spawnTransform)
        {
            Vector3 rotation = Vector3.zero;

            if (spawnTransform == GameManager.Instance.boss.SpawnPositions[0])
            {
                rotation = new Vector3(0f, 180f, 0f);
            }

            StartCoroutine(SummonShooter(spawnTransform, rotation, shooterPrefab));
        }

        #endregion

        #region SHOOTER EVENT
        private void OnEnable()
        {
            EnemyShooter.OnShooterKilled += HandleShooterKilled;
        }

        private void OnDisable()
        {
            EnemyShooter.OnShooterKilled -= HandleShooterKilled;
        }

        private void HandleShooterKilled(EnemyShooter shooter)
        {
            ShooterAmount--;
            OnSummonedEnemyKilled?.Invoke(shooter.SpawnPosition);
        }
        #endregion

        public bool CanUseSecondAttack()
        {
            return shooterAmount < shooterLimit;
        }
    }
}