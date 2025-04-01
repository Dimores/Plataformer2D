using System.Collections;
using System.Collections.Generic;
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
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }
    }
}
