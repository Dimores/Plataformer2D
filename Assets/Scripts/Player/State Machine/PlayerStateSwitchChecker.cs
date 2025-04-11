using UnityEngine;
using Orby.Managers;

namespace Orby.Player.StateMachine
{
    public class PlayerStateSwitchChecker
    {
        private Player _player;

        public PlayerStateSwitchChecker(Player player)
        {
            _player = player;
        }

        public void CheckIfIdleState()
        {
            if (InputManager.Instance.Horizontal == 0)
                _player.StateMachine.ChangeState(_player.IdleState);
        }

        public void CheckIfMovementState()
        {
            if (InputManager.Instance.Horizontal != 0)
                _player.StateMachine.ChangeState(_player.MovementState);
        }

        public void CheckIfJumpState()
        {
            if (Input.GetButtonDown(_player.playerData.jumpButtonName))
                _player.StateMachine.ChangeState(_player.JumpState);
        }

        public void CheckIfDashState()
        {
            if (Input.GetKeyDown(_player.playerData.dashKey) && !_player.IsDashOnCooldown)
                _player.StateMachine.ChangeState(_player.DashState);
        }

        public void CheckIfFallingState()
        {
            if (_player.playerData.Rb.velocity.y <= 0.1f)
                _player.StateMachine.ChangeState(_player.FallingState);
        }

        public void CheckIfAimState()
        {
            if (Input.GetKeyDown(_player.playerData.aimKey))
                _player.StateMachine.ChangeState(_player.AimState);
        }
    }
}
