using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerMovementState : PlayerState
    {
        private float horizontal;
        private float movementSpeed;

        public PlayerMovementState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
            this.movementSpeed = player.playerData.movementSpeed;
        }

        public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Move");
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            HandleMovement();
        }

        private void HandleMovement()
        {
            horizontal = Input.GetAxisRaw(player.playerData.movementAxisName);

            if (horizontal != 0)
                player.Move(movementSpeed, horizontal);
            else
                player.StateMachine.ChangeState(player.IdleState);
        }
    }
}
