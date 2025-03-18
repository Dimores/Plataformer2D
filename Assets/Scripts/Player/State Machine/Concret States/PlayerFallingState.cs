using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerFallingState : PlayerState
    {
        private float horizontal;

        public PlayerFallingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            AnimationTriggerEvent(player.playerData.triggerFalling);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            AirFallMovement();
            HandleScaleX();

            // State Transition Check
            CheckIfDashState();
            CheckIfIdleOrMovementState();

        }

        private void AirFallMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);
            player.Move(player.playerData.movementSpeed, horizontal);
        }

        private void HandleScaleX()
        {
            if (horizontal != 0)
                player.Rb.transform.DOScaleX(horizontal, player.playerData.playerSwipeDuration);
        }

        private void CheckIfIdleOrMovementState()
        {
            if (player.CheckGrounded())
            {
                player.HasDashedOnAir = false;
                if (horizontal == 0)
                    player.StateMachine.ChangeState(player.IdleState);
                else
                    playerStateMachine.ChangeState(player.MovementState);
            }
        }

        private void CheckIfDashState()
        {
            if (Input.GetKeyDown(player.playerData.dashKey) && !player.HasDashedOnAir && 
                !player.IsDashOnCooldown)
                player.StateMachine.ChangeState(player.DashState);
        }
    }
}
