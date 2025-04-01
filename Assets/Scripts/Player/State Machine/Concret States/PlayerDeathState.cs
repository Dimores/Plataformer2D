using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerName)
        {
            base.AnimationTriggerEvent(triggerName);
        }

        public override void EnterState()
        {
            base.EnterState();
            FreezePlayer();
            DisableAnimator();
            StartDissolving();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        private void FreezePlayer()
        {
            player.playerData.Rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        private void DisableAnimator()
        {
            player.playerData.characterAnimator.enabled = false;
        }

        private void StartDissolving()
        {
            player.StartCoroutine(player.dissolve.Vanish());
        }
    }
}