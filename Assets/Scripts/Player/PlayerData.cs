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
        public GameObject laserPrefab;

        [Header("Player Data - Movement Setup")]
        public Rigidbody2D Rb;
        public Transform bonesTransform;
        public Transform playerTransform;
        public Collider2D playerCollider;
        public float movementSpeed;
        public float jumpForce;
        public float variableJumpHeightMultiplier;
        public float dashForce;
        public float dashDuration;
        public float dashCooldown;
        public float direction;

        [Header("Player Data - Ground Check Setup")]
        public LayerMask groundLayer;
        public float raycastDetectionDistance = 0.4f;
        public Vector3 leftRaycastOffset;
        public Vector3 middleRaycastOffset;
        public Vector3 rightRaycastOffset;

        [Header("Player Data - Input Setup")]
        public string movementAxisName;
        public string jumpButtonName;
        public string attackKey;
        public string dashKey;
        public string aimKey;
        public string specialKey;

        [Header("Player Data - Animation Setup")]
        public string triggerIdle;
        public string triggerMovement;
        public string triggerJump;
        public string triggerFalling;
        public string triggerDash;
        public string triggerSpecial;
        public string triggerDeath;
        public float playerSwipeDuration;

        [Header("Player Data - VFX Setup")]
        public Vector3 dashVFXOffset;
        public Vector3 jumpVFXOffset;

        [Header("Player Data - Special Setup")]
        public int specialAmount;
        public int maxSpecial;

        [Header("Player Data - Volume Setup")]
        public float normalShootVolume;

        #region METHODS
        public void Move()
        {
            Rb.velocity = new Vector2(movementSpeed * direction, Rb.velocity.y);
        }

        public void Stationary()
        {
            Rb.velocity = new Vector2(movementSpeed * 0, Rb.velocity.y);
        }

        public void Jump(float jumpForce)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
        }

        public void Dash(float direction)
        {
            Rb.velocity = new Vector2(dashForce * direction, 0);
        }

        public void Special(Vector3 start, Vector3 end)
        {
            GameObject laserGO = Instantiate(laserPrefab, start, Quaternion.identity);
            Laser laser = laserGO.GetComponent<Laser>();
            laser.Initialize(start, end);
        }

        public void HandleMovement()
        {
            if (InputManager.Instance.Horizontal != 0)
            {
                Move();
                HandleScaleX();
            } else if (InputManager.Instance.Horizontal == 0)
            {
                Stationary();
            }
        }

        public void HandleScaleX()
        {
            if (Mathf.Abs(InputManager.Instance.Horizontal) < 0.1f)
                return;

            float yRotation = InputManager.Instance.Horizontal > 0 ? 0f : -180f;

            direction = yRotation == 0 ? 1f : -1f;

            bonesTransform.DORotate(new Vector3(0f, yRotation, 0f), playerSwipeDuration);
        }


        public bool CheckGrounded()
        {
            if (Physics2D.Raycast(playerTransform.position + this.leftRaycastOffset,
                    Vector2.down, this.raycastDetectionDistance, this.groundLayer)
                ||
                    Physics2D.Raycast(playerTransform.position + this.middleRaycastOffset,
                    Vector2.down, this.raycastDetectionDistance, this.groundLayer)
                ||
                    Physics2D.Raycast(playerTransform.position + this.rightRaycastOffset,
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
            this.specialAmount = 400;
        }

        #endregion
    }
}
