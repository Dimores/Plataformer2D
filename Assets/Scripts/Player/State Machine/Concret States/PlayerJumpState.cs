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
            // Check Dash State <--
            AirMovement();

        }

        private void AirMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);
            player.Move(player.playerData.movementSpeed, horizontal);
        }
    }
}
