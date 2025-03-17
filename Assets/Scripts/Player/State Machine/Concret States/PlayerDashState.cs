using System.Collections;
using System.Collections.Generic;
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

            playerInitialGravityScale = player.Rb.gravityScale;

            if (currentDashCoroutine == null)
            {
                player.Rb.gravityScale = 0f;
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
            VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.DASH, player.transform.position,
                player.playerData.dashVFXOffset, true);
            AnimationTriggerEvent(player.playerData.triggerDash);
            yield return new WaitForSeconds(player.playerData.dashDuration);
            currentDashCoroutine = null;
            CheckIfFallingState();
            CheckIfIdleState();
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
