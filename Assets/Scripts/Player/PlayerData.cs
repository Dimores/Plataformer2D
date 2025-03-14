using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Orby.Character;
using UnityEngine;
using UnityEngine.Animations;

namespace Orby.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
    public class PlayerData : CharacterData
    {
        [Header("Player Data - Combat Setup")]
        public float criticalChance;
        public float criticalDamage;
        public float attackSpeed;
        // Invencibility time here later <--
        public int projectilePerAttack;
        public int lifeStealWhenKill;

        [Header("Player Data - Movement Setup")]
        public float movementSpeed;
        public float jumpForce;
        public float dashForce;
        // Maybe time to crouch and raise right here <--

        [Header("Player Data - Ground Check Setup")]
        public LayerMask groundLayer;
        public float raycastDetectionDistance = 0.4f;
        public Vector3 leftRaycastOffset;
        public Vector3 middleRaycastOffset;
        public Vector3 rightRaycastOffset;

        [Header("Player Data - Input Setup")]
        public string movementAxisName;
        public KeyCode jumpKey;
        public KeyCode attackKey;

        [Header("Player Data - Animation Setup")]
        public string boolRun;
        public string triggerJump;
        public string boolFalling;
        public string triggerDeath;
        public float playerSwipeDuration;
    }
}
