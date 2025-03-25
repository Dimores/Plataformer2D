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

        public override void AnimationTriggerEvent(string triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            AnimationTriggerEvent(player.playerData.triggerIdle);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            player.StateMachine.PlayerStateSwitchChecker.CheckIfMovementState();
            player.StateMachine.PlayerStateSwitchChecker.CheckIfJumpState();

            player.playerData.Rb.velocity = new Vector2(0, player.playerData.Rb.velocity.y);

            player.StateMachine.PlayerStateSwitchChecker.CheckIfDashState();

        }
    }
}
