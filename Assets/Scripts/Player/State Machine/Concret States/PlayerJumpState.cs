using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerJumpState : PlayerState
    {
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
            player.playerData.Jump(player.playerData.jumpForce);
            player.PlaySmokeJumpVFX(player.transform.position, player.playerData.jumpVFXOffset);
            AnimationTriggerEvent(player.playerData.triggerJump);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            player.playerData.HandleMovement();

            player.StateMachine.PlayerStateSwitchChecker.CheckIfFallingState();
            player.StateMachine.PlayerStateSwitchChecker.CheckIfDashState();
        }
    }
}