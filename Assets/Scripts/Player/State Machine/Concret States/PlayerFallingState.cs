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
            playerStateMachine.PlayerStateSwitchChecker.CheckIfSpecialState();
        }

        private void CheckIfIdleOrMovementState()
        {
            if (player.playerData.CheckGrounded())
            {
                AudioManager.Instance.PlayAudioByTypeWithRandomPitch(AudioManager.AudioType.FALL,
                    new Vector2(0.96f, 1.1f), 0.55f);
                player.PlaySmokeFallVFX(player.transform.position, player.playerData.fallVFXOffset);
                player.HasDashedOnAir = false;
                if (InputManager.Instance.Horizontal == 0)
                    player.StateMachine.ChangeState(player.IdleState);
                else
                    playerStateMachine.ChangeState(player.MovementState);
            }
        }

        private void CheckIfDashState()
        {
            if (Input.GetButtonDown(player.playerData.dashKey) && !player.HasDashedOnAir && 
                !player.IsDashOnCooldown)
                player.StateMachine.ChangeState(player.DashState);
        }
    }
}