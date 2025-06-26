using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerMovementState : PlayerState
    {
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
            player.playerData.HandleMovement();

            player.StateMachine.PlayerStateSwitchChecker.CheckIfJumpState();
            player.StateMachine.PlayerStateSwitchChecker.CheckIfIdleState();
            player.StateMachine.PlayerStateSwitchChecker.CheckIfDashState();
            player.StateMachine.PlayerStateSwitchChecker.CheckIfAimState();
            player.StateMachine.PlayerStateSwitchChecker.CheckIfSpecialState();
        }
    }
}
