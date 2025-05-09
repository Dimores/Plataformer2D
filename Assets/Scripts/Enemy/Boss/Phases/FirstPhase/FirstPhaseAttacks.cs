using System.Collections;
using System.Collections.Generic;
using Orby.Managers;
using UnityEngine;

namespace Orby.Enemy.Boss.Phases.FirstPhase
{
    public class FirstPhaseAttacks : MonoBehaviour
    {
        [Header("Limits")]
        public int consecutiveFireballLimit = 4;

        [Header("Shoot")]
        [SerializeField] private Fireball fireballPrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float timeBetweenShoots;
        [SerializeField] private float reloadTime;
        [SerializeField] private float fireballSpeed;

        private int fireballCount = 0;
        private Vector3 playerDirection;
        private Coroutine currentFireballCoroutine;

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
                Vector3 playerCenter = GameManager.Instance.Player.transform.position + new Vector3(0, 0.5f, 0);
                Vector3 rawDirection = playerCenter - shootPoint.position;
                rawDirection.z = 0f;
                playerDirection = rawDirection.normalized;



                var firstFireball = Instantiate(fireballPrefab);
                firstFireball.transform.position = shootPoint.position;
                firstFireball.Direction = playerDirection * fireballSpeed;

                yield return new WaitForSeconds(timeBetweenShoots);

                var secondFireball = Instantiate(fireballPrefab);
                secondFireball.transform.position = shootPoint.position;
                secondFireball.Direction = playerDirection * fireballSpeed;

                fireballCount++;

                yield return new WaitForSeconds(reloadTime);
                currentFireballCoroutine = null;
            }
        }
    }
}
