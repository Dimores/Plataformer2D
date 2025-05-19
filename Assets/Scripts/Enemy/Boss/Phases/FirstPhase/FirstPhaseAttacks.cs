using System.Collections;
using System.Collections.Generic;
using Orby.Managers;
using UnityEngine;

namespace Orby.Enemy.Boss.Phases.FirstPhase
{
    public class FirstPhaseAttacks : MonoBehaviour
    {
        public EnemyData enemyData;

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
                enemyData.characterAnimator.SetTrigger("Fireball");

                // Espera a animação acontecer e o evento de animação chamar ShootFireball()
                // Este tempo precisa ser maior que a duração da animação
                yield return new WaitUntil(() => fireballCount >= consecutiveFireballLimit);
            }

            currentFireballCoroutine = null;
            enemyData.characterAnimator.SetTrigger("Idle");
        }




        // Chamado pela animação no momento exato do disparo
        public void ShootFireball()
        {
            // Impede disparos além do limite
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



    }
}
