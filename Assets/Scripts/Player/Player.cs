using UnityEngine;
using DG.Tweening;
using Orby.Interfaces;
using Orby.Player.StateMachine;
using Orby.Player.StateMachine.ConcretStates;
using System.Collections;
using UnityEngine.VFX;
using Orby.Managers;
using Orby.Gun;

namespace Orby.Player
{
    public class Player : MonoBehaviour, IDamageable, IMoveable, IKillable
    {
        public PlayerData playerData;
        public VisualEffect smokeVisualEffect;
        public VisualEffect smokeJumpVisualEffect;

        public GunBase GunBase;

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
        #endregion

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();
            StateMachine.PlayerStateSwitchChecker = new PlayerStateSwitchChecker(this);

            IdleState = new PlayerIdleState(this, StateMachine);
            MovementState = new PlayerMovementState(this, StateMachine);
            JumpState = new PlayerJumpState(this, StateMachine);
            FallingState = new PlayerFallingState(this, StateMachine);
            DashState = new PlayerDashState(this, StateMachine);
        }

        private void Start()
        {
            playerData.Rb = GetComponent<Rigidbody2D>();
            playerData.characterAnimator = GetComponent<Animator>();
            playerData.PlayerTransform = this.transform;

            MaxHealth = playerData.life;
            CurrentHealth = MaxHealth;

            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentPlayerState.FrameUpdate();
        }

        // Raycast Debug
        //private void LateUpdate()
        //{
        //    Debug.DrawRay(transform.position + playerData.leftRaycastOffset, Vector2.down * playerData.raycastDetectionDistance, Color.red);
        //    Debug.DrawRay(transform.position + playerData.middleRaycastOffset, Vector2.down * playerData.raycastDetectionDistance, Color.red);
        //    Debug.DrawRay(transform.position + playerData.rightRaycastOffset, Vector2.down * playerData.raycastDetectionDistance, Color.red);
        //}


        public void Damage(int damageAmount)
        {
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
                Kill();
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

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

        public void PlaySmokeVFX(Vector3 position, Vector3 offset,
            bool willDestroy = true)
        {
            var item = Instantiate(smokeVisualEffect, null);
            var vfx = item.GetComponent<VisualEffect>();

            var scaleX = Mathf.Round(transform.localScale.x);
            var direction = Mathf.Approximately(transform.localScale.x, 1f) ? -17 : 17;
            float offsetX = Mathf.Approximately(transform.localScale.x, 1f) ? 2.5f : -2.5f;

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

        public void Move(float direction)
        {
            playerData.Move(direction);
        }
    }
}