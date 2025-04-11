using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Orby.Character;
using Orby.Managers;
using UnityEngine;
using UnityEngine.Animations;

namespace Orby.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
    public class PlayerData : CharacterData
    {
        [Header("Player Data - Combat Setup")]
        public float attackSpeed;
        public float projectileSpeed;
        // Invencibility time here later <--

        [Header("Player Data - Movement Setup")]
        public Rigidbody2D Rb;
        public Transform PlayerTransform;
        public Collider2D playerCollider;
        public float movementSpeed;
        public float jumpForce;
        public float variableJumpHeightMultiplier;
        public float dashForce;
        public float dashDuration;
        public float dashCooldown;
        // Maybe time to crouch and raise right here <--

        [Header("Player Data - Ground Check Setup")]
        public LayerMask groundLayer;
        public float raycastDetectionDistance = 0.4f;
        public Vector3 leftRaycastOffset;
        public Vector3 middleRaycastOffset;
        public Vector3 rightRaycastOffset;

        [Header("Player Data - Input Setup")]
        public string movementAxisName;
        public string jumpButtonName;
        public KeyCode attackKey;
        public KeyCode dashKey;
        public KeyCode aimKey;

        [Header("Player Data - Animation Setup")]
        public string triggerIdle;
        public string triggerMovement;
        public string triggerJump;
        public string triggerFalling;
        public string triggerDash;
        public string triggerDeath;
        public float playerSwipeDuration;

        [Header("Player Data - VFX Setup")]
        public Vector3 dashVFXOffset;
        public Vector3 jumpVFXOffset;

        [Header("Player Data - Points Setup")]
        public int specialAmount;
        public int maxSpecial;

        #region METHODS
        public void Move(float direction)
        {
            Rb.velocity = new Vector2(movementSpeed * direction, Rb.velocity.y);
        }

        public void Jump(float jumpForce)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
        }

        public void Dash(float dashForce, float direction)
        {
            Rb.velocity = new Vector2(dashForce * direction, 0);
        }

        public void HandleMovement()
        {
            if (InputManager.Instance.Horizontal != 0)
            {
                Move(InputManager.Instance.Horizontal);
                HandleScaleX();
            } else if (InputManager.Instance.Horizontal == 0)
            {
                Move(0);
            }
        }

        private void HandleScaleX()
        {
            Rb.transform.DOScaleX(InputManager.Instance.Horizontal,
                this.playerSwipeDuration);
        }

        public bool CheckGrounded()
        {
            if (Physics2D.Raycast(PlayerTransform.position + this.leftRaycastOffset,
                    Vector2.down, this.raycastDetectionDistance, this.groundLayer)
                ||
                    Physics2D.Raycast(PlayerTransform.position + this.middleRaycastOffset,
                    Vector2.down, this.raycastDetectionDistance, this.groundLayer)
                ||
                    Physics2D.Raycast(PlayerTransform.position + this.rightRaycastOffset,
                    Vector2.down, this.raycastDetectionDistance, this.groundLayer))
                return true;

            return false;
        }

        public void AddSpecial(int value)
        {
            if (specialAmount + value >= maxSpecial) 
                specialAmount = maxSpecial;
            else
                specialAmount += value;
        }

        public void RemoveSpecial(int value)
        {
            this.specialAmount -= value;
        }

        public void ResetSpecial()
        {
            this.specialAmount = 0;
        }

        #endregion
    }
}
