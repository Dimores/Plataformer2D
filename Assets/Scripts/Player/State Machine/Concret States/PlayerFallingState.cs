using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerFallingState : PlayerState
    {
        private float horizontal;

        public PlayerFallingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Falling");
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // State Transition Check
            // Dash
            CheckIfIdleState();


            AirFallMovement();
        }

        private void AirFallMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);
            player.Move(player.playerData.movementSpeed, horizontal);
        }

        private void CheckIfIdleState()
        {
            if (player.CheckGrounded())
                player.StateMachine.ChangeState(player.IdleState);
        }

    }
}
