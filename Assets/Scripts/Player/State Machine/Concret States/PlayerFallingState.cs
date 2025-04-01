using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Orby.Managers;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerFallingState : PlayerState
    {
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

            if (InputManager.Instance.Horizontal == 0)
            {
                player.playerData.Rb.velocity = new Vector2(0, player.playerData.Rb.velocity.y);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            player.playerData.HandleMovement();

            CheckIfDashState();
            CheckIfIdleOrMovementState();
        }

        private void CheckIfIdleOrMovementState()
        {
            if (player.playerData.CheckGrounded())
            {
                player.HasDashedOnAir = false;
                if (InputManager.Instance.Horizontal == 0)
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