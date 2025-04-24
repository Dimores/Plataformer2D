using UnityEngine;
using DG.Tweening;
using Orby.Interfaces;
using Orby.Player.StateMachine;
using Orby.Player.StateMachine.ConcretStates;
using System.Collections;
using UnityEngine.VFX;
using Orby.Managers;
using Orby.Gun;
using Orby.UI;
using Orby.Utils;
using NUnit.Framework;
using System.Collections.Generic;

namespace Orby.Player
{
    public class Player : MonoBehaviour, IDamageable, IMoveable, IKillable
    {
        public PlayerData playerData;
        public VisualEffect smokeVisualEffect;
        public VisualEffect smokeJumpVisualEffect;

        public GunBase GunBase;
        public Dissolve dissolve;

        public Transform bonesTransform;

        public bool HasDashedOnAir { get; set; } = false;
        public bool IsDashOnCooldown { get; private set; }

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }

        #region State Machine Variables
        public PlayerStateMachine StateMachine { get; set; }
        public PlayerIdleState IdleState { get; set; }
        public PlayerMovementState MovementState { get; set; }
        public PlayerJumpState JumpState { get; set; }
        public PlayerFallingState FallingState { get; set; }
        public PlayerDashState DashState { get; set; }
        public PlayerDeathState DeathState { get; set; }
        public PlayerAimState AimState { get; set; }
        #endregion

        private bool _invincibility = false;
        
        private void Awake()
        {
            StateMachine = new PlayerStateMachine();
            StateMachine.PlayerStateSwitchChecker = new PlayerStateSwitchChecker(this);

            IdleState = new PlayerIdleState(this, StateMachine);
            MovementState = new PlayerMovementState(this, StateMachine);
            JumpState = new PlayerJumpState(this, StateMachine);
            FallingState = new PlayerFallingState(this, StateMachine);
            DashState = new PlayerDashState(this, StateMachine);
            DeathState = new PlayerDeathState(this, StateMachine);
            AimState = new PlayerAimState(this, StateMachine);
        }

        private void Start()
        {
            playerData.Rb = GetComponent<Rigidbody2D>();
            playerData.characterAnimator = GetComponent<Animator>();

            playerData.playerCollider = GetComponent<Collider2D>();

            dissolve = GetComponent<Dissolve>();

            playerData.playerTransform = this.transform;
            playerData.bonesTransform = bonesTransform;

            MaxHealth = playerData.life;
            CurrentHealth = MaxHealth;

            StateMachine.Initialize(IdleState);

            // Set initial direction
            playerData.direction = 1;
        }

        private void Update()
        {
            StateMachine.CurrentPlayerState.FrameUpdate();

            if (Input.GetKeyDown(KeyCode.R))
            {
                Damage(1);
            }
        }


        // Raycast Debug
        //private void LateUpdate()
        //{
        //    Debug.DrawRay(transform.position + playerData.leftRaycastOffset, Vector2.down * playerData.raycastDetectionDistance, Color.red);
        //    Debug.DrawRay(transform.position + playerData.middleRaycastOffset, Vector2.down * playerData.raycastDetectionDistance, Color.red);
        //    Debug.DrawRay(transform.position + playerData.rightRaycastOffset, Vector2.down * playerData.raycastDetectionDistance, Color.red);
        //}

        public void Move(float direction)
        {
            playerData.Move(direction);
        }

        public void Dash()
        {
            playerData.Dash(playerData.direction);
        }

        public void Freeze()
        {
            playerData.Rb.constraints = RigidbodyConstraints2D.FreezePosition;
            playerData.characterAnimator.enabled = false;
        }

        public void Unfreeze()
        {
            playerData.Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerData.characterAnimator.enabled = true;
        }

        public void Damage(int damageAmount)
        {
            if (_invincibility) return; 

            CurrentHealth -= damageAmount;

            AudioManager.Instance.PlayAudioByType(AudioManager.AudioType.DAMAGE, 0.9f);
            UiLifeManager.Instance.UpdateLifeOnUi(CurrentHealth);

            if (CurrentHealth > 0) {
                StartCoroutine(FreezePlayerCoroutine(0.2f));
                StartCoroutine(BlinkEffect());
            }

            if (CurrentHealth <= 0)
                Kill();
        }

        private void WillCollideWithEnemy(bool value)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemy");

            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, !value);
        }



        public void Kill()
        {
            StateMachine.ChangeState(DeathState);
        }

        private IEnumerator FreezePlayerCoroutine(float freezeTime)
        {
            Freeze();
            yield return new WaitForSeconds(freezeTime);
            Unfreeze();
        }

        #region DOTWEEN
        private IEnumerator BlinkEffect()
        {
            _invincibility = true;
            WillCollideWithEnemy(false); 

            float blinkDuration = 0.2f;
            int blinkCount = 4;

            int completedEffects = 0; 

            foreach (var spriteRenderer in dissolve.SpriteRenderers)
            {
                spriteRenderer.DOFade(0, blinkDuration)
                    .SetLoops(blinkCount * 2, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        spriteRenderer.color = Color.white; 
                        completedEffects++; 

                        
                        if (completedEffects == dissolve.SpriteRenderers.Length)
                        {
                            _invincibility = false;
                            WillCollideWithEnemy(true);
                        }
                    });
            }

            yield return null; 
        }

        #endregion

        #region DASH
        public void StartDashCooldown(float cooldownTime)
        {
            IsDashOnCooldown = true;
            StartCoroutine(DashCooldownCoroutine(cooldownTime));
        }

        private IEnumerator DashCooldownCoroutine(float cooldownTime)
        {
            yield return new WaitForSeconds(cooldownTime);
            IsDashOnCooldown = false;
        }
        #endregion

        #region VFX
        public void PlaySmokeVFX(Vector3 position, Vector3 offset,
            bool willDestroy = true)
        {
            var item = Instantiate(smokeVisualEffect, null);
            var vfx = item.GetComponent<VisualEffect>();

            var direction = playerData.direction == 1f ? -17 : 17;
            float offsetX = playerData.direction == 1f ? 2.5f : -1.5f;

            item.transform.position = position + new Vector3(offsetX, offset.y, offset.z);
            vfx.SetVector3("Direction", new Vector3(direction, 0, 0));


            if (willDestroy)
                Destroy(item.gameObject, 3f);
        }

        public void PlaySmokeJumpVFX(Vector3 position, Vector3 offset, 
            bool willDestroy = true)
        {
            var item = Instantiate(smokeJumpVisualEffect, null);
            item.transform.position = position + offset;

            if (willDestroy)
                Destroy(item.gameObject, 3f);
        }
        #endregion
    }
}