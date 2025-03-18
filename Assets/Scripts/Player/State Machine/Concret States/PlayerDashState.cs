using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VFXManager;

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

            player.StartDashCooldown(player.playerData.dashCooldown); 

            playerInitialGravityScale = player.Rb.gravityScale;
            player.Rb.gravityScale = 0f;

            if (currentDashCoroutine == null)
            {
                currentDashCoroutine = player.StartCoroutine(Dash());
            }
        }


        public override void ExitState()
        {
            base.ExitState();
            player.Rb.gravityScale = playerInitialGravityScale;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        IEnumerator Dash()
        {
            player.Dash(player.playerData.dashForce, player.transform.localScale.x);
            AnimationTriggerEvent(player.playerData.triggerDash);
            player.PlaySmokeVFX(player.transform.position,
                player.playerData.dashVFXOffset, true);
            yield return new WaitForSeconds(player.playerData.dashDuration);
            CheckIfFallingState();
            CheckIfIdleState();
            currentDashCoroutine = null;
        }

        // Dash on air
        private void CheckIfFallingState()
        {
            if (!player.CheckGrounded())
            {
                player.HasDashedOnAir = true;
                player.StateMachine.ChangeState(player.FallingState);
            }
        }

        private void CheckIfIdleState()
        {
            if (player.CheckGrounded())
            {
                player.HasDashedOnAir = false;
                player.StateMachine.ChangeState(player.IdleState);
            }
        }

        private void CheckIfMovementState()
        {

        }


    }
}
