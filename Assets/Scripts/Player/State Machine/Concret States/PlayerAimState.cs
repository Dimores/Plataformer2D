using System.Collections;
using System.Collections.Generic;
using Orby.Gun;
using Orby.Managers;
using Orby.Player.StateMachine;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerAimState : PlayerState
    {
        public PlayerAimState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerName)
        {
            base.AnimationTriggerEvent(triggerName);
        }

        public override void EnterState()
        {
            base.EnterState();
            AnimationTriggerEvent(player.playerData.triggerIdle);
        }

        public override void ExitState()
        {
            base.ExitState();
            player.GunBase.ExitAim();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            player.GunBase.Aim();
            player.playerData.Rb.velocity = new Vector2(0, player.playerData.Rb.velocity.y);

            player.StateMachine.PlayerStateSwitchChecker.CheckIfJumpState();
            CheckIfStateSwitchGround();

        }

        private void CheckIfStateSwitchGround()
        {
            if (Input.GetKeyUp(player.playerData.aimKey))
            {
                if (InputManager.Instance.Horizontal == 0)
                    player.StateMachine.ChangeState(player.IdleState);
                else
                    player.StateMachine.ChangeState(player.MovementState);
            }else if (Input.GetKeyDown(player.playerData.dashKey))
                player.StateMachine.ChangeState(player.DashState);
        }
    }
}
