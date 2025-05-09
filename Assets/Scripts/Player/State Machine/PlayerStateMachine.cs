using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Player.StateMachine
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentPlayerState { get; set; }

        public PlayerStateSwitchChecker PlayerStateSwitchChecker { get; set; }

        public void Initialize(PlayerState startingState)
        {
            CurrentPlayerState = startingState;
            CurrentPlayerState.EnterState();
        }
        
        public void ChangeState(PlayerState newState)
        {
            CurrentPlayerState.ExitState();
            CurrentPlayerState = newState;
            CurrentPlayerState.EnterState();
        }
    }
}
