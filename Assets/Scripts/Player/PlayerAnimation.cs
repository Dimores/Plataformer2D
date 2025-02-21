using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("VFX")]
    public Vector3 walkVFXOffset;
    public Vector3 jumpVFXOffset;
    public Vector3 fallVFXOffset;

    // Walk Dust
    private ParticleSystem _walkVFX;

    // Animator
    private Animator _currentPlayer;

    // Player
    private Player _player;

    #region PROPERTIES
    public Animator CurrentPlayer { get => _currentPlayer; set => _currentPlayer = value; }
    #endregion


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _player = GetComponent<Player>();
        _currentPlayer = Instantiate(_player.playerData.player, transform);
        _walkVFX = VFXManager.Instance.PlayAndGetPermanentVFXByType(VFXManager.VFXType.WALK,
            this.transform.position, walkVFXOffset, this.transform);
    }

    private void Update()
    {
        walkVFXControl();
        CheckSprintAnimation();
        CheckRunAnimation();
        CheckJumpAnimation();
        CheckFallAnimation();
    }


    private void walkVFXControl()
    {
        if (_walkVFX != null)
        {
            if (!_player.IsGrounded || !_player.CanControl)
            {
                _walkVFX.Stop();
            }
            else if (!_walkVFX.isPlaying)
            {
                _walkVFX.Play();
            }
        }
    }

    private void CheckSprintAnimation()
    {
        if (_player.CurrentSpeed == _player.SpeedRun)
        {
            if (_player.Horizontal == 0)
                _currentPlayer.SetBool(_player.playerData.boolSprint.value, false);
            else
                _currentPlayer.SetBool(_player.playerData.boolSprint.value, true);
        }
        else
        {
            _currentPlayer.SetBool(_player.playerData.boolSprint.value, false);
        }
    }

    private void CheckRunAnimation()
    {
        if (_player.Horizontal == 0)
        {
            _currentPlayer.SetBool(_player.playerData.boolRun.value, false);
        }
        else
        {
            _currentPlayer.SetBool(_player.playerData.boolRun.value, true);
        }
    }

    private void CheckJumpAnimation()
    {
        if (Input.GetKeyDown(_player.playerData.jump.value) && _player.IsGrounded)
        {
            _currentPlayer.SetTrigger(_player.playerData.triggerJump.value);
        }
    }

    private void CheckFallAnimation()
    {
        if (!_player.WasFalling && _player.IsGrounded)
        {
            _currentPlayer.SetBool(_player.playerData.boolFalling.value, false);
        } else if (!_player.WasFalling && !_player.IsGrounded && _player.Rb.velocity.y < -0.1f)
        {
            _currentPlayer.SetBool(_player.playerData.boolFalling.value, true);
        }
    }
}
