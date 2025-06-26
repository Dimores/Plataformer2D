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
            player.Freeze();
            DisableAnimator();
            StartDissolving();
            DisableCollider();
            DisableGun();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        private void DisableGun()
        {
            player.GunBase.enabled = false;
        }

        private void DisableAnimator()
        {
            player.animator.enabled = false;
        }

        private void DisableCollider()
        {
            player.GetComponent<Collider2D>().enabled = false;
        }

        private void StartDissolving()
        {
            player.StartCoroutine(player.dissolve.Vanish());
        }
    }
}