using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerMovementState : PlayerState
    {
        private float horizontal;

        public PlayerMovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            AnimationTriggerEvent(player.playerData.triggerMovement);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            HandleMovement();
            HandleScaleX();

            // State Transition Check
            CheckIfJumpState();
            CheckIfIdleState();
            CheckIfDashState();

        }

        private void HandleMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);

            if (horizontal != 0)
                player.Move(player.playerData.movementSpeed, horizontal);
        }

        private void HandleScaleX()
        {
            if (horizontal != 0)
                player.Rb.transform.DOScaleX(horizontal, player.playerData.playerSwipeDuration);
        }

        private void CheckIfIdleState()
        {
            if (horizontal == 0)
                player.StateMachine.ChangeState(player.IdleState);
        }

        private void CheckIfJumpState()
        {
            if (Input.GetKeyDown(player.playerData.jumpKey))
                player.StateMachine.ChangeState(player.JumpState);
        }

        private void CheckIfDashState()
        {
            if (Input.GetKeyDown(player.playerData.dashKey))
                player.StateMachine.ChangeState(player.DashState);
        }
    }
}
