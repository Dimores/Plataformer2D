using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerMovementState : PlayerState
    {
        private float horizontal;

        public PlayerMovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // State Transition Check
            CheckIfJumpState();

            HandleMovement();
        }

        private void HandleMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);

            if (horizontal != 0)
                player.Move(player.playerData.movementSpeed, horizontal);
            else
                player.StateMachine.ChangeState(player.IdleState);
        }

        private void CheckIfJumpState()
        {
            if (Input.GetKeyDown(player.playerData.jumpKey))
                player.StateMachine.ChangeState(player.JumpState);
        }
    }
}
