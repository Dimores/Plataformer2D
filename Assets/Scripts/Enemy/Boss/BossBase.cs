using System.Collections;
using System.Collections.Generic;
using Orby.Enemy.Boss.Phases.FirstPhase;
using Orby.Interfaces.IPhase;
using Orby.Managers;
using UnityEngine;

namespace Orby.Enemy.Boss
{
    public class BossBase : MonoBehaviour
    {
        [Header("Phase Scripts")]
        [SerializeField] private FirstPhaseAttacks firstPhase;
        [SerializeField] private SecondPhaseAttacks secondPhase;
        [SerializeField] private FirstPhaseAttacks thirdPhase;

        [Header("Summons - Spawn positions")]
        [SerializeField] private List<Transform> spawnPositions; // Left and Right

        [Header("Limits")]
        [SerializeField] private float timeBetweenAttacks = 1.0f;
        private float attackCooldown = 0f;
        private bool isAttacking = false;

        private List<Transform> usedSpawnPositions = new List<Transform>();

        public List<Transform> SpawnPositions { get => spawnPositions; private set => spawnPositions = value; }

        private IPhase currentPhase;

        void Start()
        {
            currentPhase = firstPhase;
        }

        private void OnEnable()
        {
            if (currentPhase != null)
            {
                currentPhase.OnAttackFinished += HandleAttackFinished;
                currentPhase.OnSummonedEnemyKilled += ReleaseSpawnPosition;
            }
        }

        private void OnDisable()
        {
            if (currentPhase != null)
            {
                currentPhase.OnAttackFinished -= HandleAttackFinished;
                currentPhase.OnSummonedEnemyKilled -= ReleaseSpawnPosition;
            }
        }

        void Update()
        {
            if (!isAttacking && GameManager.Instance.GameOver == false)
            {
                attackCooldown -= Time.deltaTime;

                if (attackCooldown <= 0f)
                {
                    TryAttack();
                    attackCooldown = timeBetweenAttacks;
                }
            }
        }

        private void TryAttack()
        {
            isAttacking = true;

            if (!currentPhase.CanUseSecondAttack())
            {
                currentPhase.FirstAttack();
                return;
            }

            float randomValue = Random.value;

            if (randomValue <= 0.6f)
            {
                currentPhase.FirstAttack();
                return;
            }
            else
            {
                List<Transform> availableSpawns = spawnPositions.FindAll(pos => !usedSpawnPositions.Contains(pos));

                if (availableSpawns.Count > 0)
                {
                    Transform randomSpawn = availableSpawns[Random.Range(0, availableSpawns.Count)];
                    usedSpawnPositions.Add(randomSpawn);
                    currentPhase.SecondAttack(randomSpawn);
                    StartCoroutine(ResetAttackFlag(2f));
                }
                else
                {
                    currentPhase.FirstAttack();
                    return;
                }
            }
        }

        private IEnumerator ResetAttackFlag(float delay)
        {
            yield return new WaitForSeconds(delay);
            isAttacking = false;
        }

        public void ReleaseSpawnPosition(Transform position)
        {
            if (usedSpawnPositions.Contains(position))
            {
                usedSpawnPositions.Remove(position);
            }
        }

        // Animation, change by life percentage
        private void ChangePhase(IPhase newPhase)
        {
            if (currentPhase != null)
            {
                currentPhase.OnAttackFinished -= HandleAttackFinished;
                currentPhase.OnSummonedEnemyKilled -= ReleaseSpawnPosition;
            }

            currentPhase = newPhase;

            currentPhase.OnAttackFinished += HandleAttackFinished;
            currentPhase.OnSummonedEnemyKilled += ReleaseSpawnPosition;
        }

        private void HandleAttackFinished()
        {
            isAttacking = false;
        }
    }
}
