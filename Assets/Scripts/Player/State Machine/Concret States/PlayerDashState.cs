using System.Collections;
using System.Collections.Generic;
using Orby.Managers;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerDashState : PlayerState
    {
        private Coroutine currentDashCoroutine;
        private float playerInitialGravityScale;

        public PlayerDashState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerName)
        {
            base.AnimationTriggerEvent(triggerName);
        }

        public override void EnterState()
        {
            base.EnterState();

            player.GunBase.enabled = false;

            player.StartDashCooldown(player.playerData.dashCooldown); 

            playerInitialGravityScale = player.playerData.Rb.gravityScale;
            player.playerData.Rb.gravityScale = 0f;

            if (currentDashCoroutine == null)
            {
                currentDashCoroutine = player.StartCoroutine(Dash());
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            player.playerData.Rb.gravityScale = playerInitialGravityScale;
            player.GunBase.enabled = true;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        IEnumerator Dash()
        {
            player.playerData.Dash(player.playerData.dashForce, player.transform.localScale.x);
            AnimationTriggerEvent(player.playerData.triggerDash);
            player.PlaySmokeVFX(player.transform.position,
                player.playerData.dashVFXOffset, true);
            yield return new WaitForSeconds(player.playerData.dashDuration);
            CheckIfFallingStateAfterDash();
            CheckIfIdleStateAfterDash();
            CheckIfMovementStateAfterDash();
            currentDashCoroutine = null;
        }

        // Dash on air
        private void CheckIfFallingStateAfterDash()
        {
            if (!player.playerData.CheckGrounded())
            {
                player.HasDashedOnAir = true;
                player.StateMachine.ChangeState(player.FallingState);
            }
        }

        private void CheckIfIdleStateAfterDash()
        {
            if (player.playerData.CheckGrounded() && InputManager.Instance.Horizontal == 0)
            {
                player.HasDashedOnAir = false;
                player.StateMachine.ChangeState(player.IdleState);
            }
        }

        private void CheckIfMovementStateAfterDash()
        {
            if (player.playerData.CheckGrounded() && InputManager.Instance.Horizontal != 0)
            {
                player.HasDashedOnAir = false;
                player.StateMachine.ChangeState(player.MovementState);
            }
        }
    }
}
