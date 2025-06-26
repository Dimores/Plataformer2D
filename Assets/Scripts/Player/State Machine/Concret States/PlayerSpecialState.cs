using System.Collections;
using System.Collections.Generic;
using Orby.Managers;
using Orby.Player.StateMachine;
using UnityEngine;

namespace Orby.Player.StateMachine.ConcretStates
{
    public class PlayerSpecialState : PlayerState
    {
        private Laser laser;
        public PlayerSpecialState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void AnimationTriggerEvent(string triggerName)
        {
            base.AnimationTriggerEvent(triggerName);
        }

        public override void EnterState()
        {
            base.EnterState();
            AnimationTriggerEvent(player.playerData.triggerSpecial);

            laser = player.playerData.laserPrefab.GetComponent<Laser>();

            SpecialManager.Instance.RemoveSpecial(100);

            player.GunBase.FlipShootPoints();

            player.playerData.Special(player.GunBase.GetShootPoint().position, 
                player.GunBase.GetEndShootPoint().position);
            player.GunBase.enabled = false;
            player.StartCoroutine(WaitLaserTime());
        }

        public override void ExitState()
        {
            base.ExitState();
            player.GunBase.enabled = true;
            player.GunBase.SetAnimationTrigger(player.GunBase.defaultTrigger);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            player.playerData.Rb.velocity = new Vector2 (0, 0); // Stop in air too
        }

        private IEnumerator WaitLaserTime()
        {
            yield return new WaitForSeconds(laser.goTime + laser.backTime);
            player.StateMachine.ChangeState(player.IdleState);
        }
    }
}
