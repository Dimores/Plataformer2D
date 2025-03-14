using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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
            CheckIfMoveState();
            CheckIfJumpState();

            player.Rb.velocity = new Vector2(0, player.Rb.velocity.y);
        }

        private void CheckIfMoveState()
        {
            if (Input.GetAxisRaw(player.playerData.movementAxisName) != 0)
                player.StateMachine.ChangeState(player.MovementState);
        }

        private void CheckIfJumpState()
        {
            if (Input.GetKeyDown(player.playerData.jumpKey))
                player.StateMachine.ChangeState(player.JumpState);
        }
    }
}
