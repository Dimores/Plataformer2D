using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerJumpState : PlayerState
    {
        private float horizontal;

        public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            player.Jump(player.playerData.jumpForce);
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
            CheckIfFallingState();

            AirJumpMovement();
        }

        private void AirJumpMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);
            player.Move(player.playerData.movementSpeed, horizontal);
        }

        private void CheckIfFallingState()
        {
            if (player.Rb.velocity.y <= 0.1f)
                player.StateMachine.ChangeState(player.FallingState);
        }
    }
}
