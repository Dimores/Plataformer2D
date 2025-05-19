using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orby.Enemy.Boss.Phases.FirstPhase;

namespace Orby.Enemy.Boss
{
    public class Boss : EnemyBase
    {
        [Header("Phase scripts")]
        [SerializeField] private FirstPhaseAttacks firstPhase;

        [Header("Runner - Spawn positions")]
        [SerializeField] private List<Transform> spawnPositions; // Left and Right

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                firstPhase.StartFireballAttack();
            }
        }
    }
}
