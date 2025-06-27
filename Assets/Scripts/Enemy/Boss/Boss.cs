using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orby.Enemy.Boss.Phases.FirstPhase;
using Orby.Managers;


namespace Orby.Enemy.Boss
{
    public class Boss : EnemyBase
    {
        [Header("Phase scripts")]
        [SerializeField] private FirstPhaseAttacks firstPhase;

        [Header("Summons - Spawn positions")]
        [SerializeField] private List<Transform> spawnPositions; // Left and Right

        [Header("Limits")]
        [SerializeField] private float timeBetweenAttacks = 1.0f;
        private float attackCooldown = 0f;
        private bool isAttacking = false;

        private List<Transform> usedSpawnPositions = new List<Transform>();

        public List<Transform> SpawnPositions { get => spawnPositions; private set => spawnPositions = value; }


        private void OnEnable()
        {
            firstPhase.OnAttackFinished += HandleAttackFinished;
        }

        private void OnDisable()
        {
            firstPhase.OnAttackFinished -= HandleAttackFinished;
        }

        private void HandleAttackFinished()
        {
            isAttacking = false;
        }


        protected override void Start()
        {
            base.Start();
        }

        private void TryAttack()
        {
            isAttacking = true;

            if (firstPhase.ShooterAmount >= 2)
            {
                firstPhase.StartFireballAttack();
                return;
            }

            float randomValue = Random.value; 

            if (randomValue <= 0.6f)
            {
                firstPhase.StartFireballAttack();
                return;
            }
            else
            {
                List<Transform> availableSpawns = spawnPositions.FindAll(pos => !usedSpawnPositions.Contains(pos));

                if (availableSpawns.Count > 0)
                {
                    Transform randomSpawn = availableSpawns[Random.Range(0, availableSpawns.Count)];
                    usedSpawnPositions.Add(randomSpawn);
                    firstPhase.SummonShooterWrapper(randomSpawn);
                    StartCoroutine(ResetAttackFlag(2f));
                }
                else
                {
                    firstPhase.StartFireballAttack();
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
    }
}
