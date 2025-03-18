using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerJumpState : PlayerState
    {
        private float horizontal;

        public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            player.Jump(player.playerData.jumpForce);
            AnimationTriggerEvent(player.playerData.triggerJump);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // State Transition Check
            // Check Dash State <--
            AirJumpMovement();
            HandleScaleX();

            CheckIfFallingState();
            CheckIfDashState();
        }

        private void AirJumpMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);
            player.Move(player.playerData.movementSpeed, horizontal);
        }

        private void HandleScaleX()
        {
            if (horizontal != 0)
                player.Rb.transform.DOScaleX(horizontal, player.playerData.playerSwipeDuration);
        }

        private void CheckIfFallingState()
        {
            if (player.Rb.velocity.y <= 0.1f)
                player.StateMachine.ChangeState(player.FallingState);
        }

        private void CheckIfDashState()
        {
            if (Input.GetKeyDown(player.playerData.dashKey) && !player.IsDashOnCooldown)
                player.StateMachine.ChangeState(player.DashState);
        }
    }
}
