using System.Collections;
using System.Collections.Generic;
using Orby.Character;
using UnityEngine;

namespace Orby.Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
    public class EnemyData : CharacterData
    {
        [Header("Enenmy Data - Combat Setup")]
        public float attackSpeed;

        [Header("Enenmy Data - Movement Setup")]
        public float movementSpeed;

        [Header("Enemy Data - Animation Setup")]
        public string boolRun;
        public string triggerDeath;
    }
}
