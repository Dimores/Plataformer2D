using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    private Rigidbody2D _myRigidbody;
    private float _speedRun;
    private float _currentSpeed;
    private float _currentScaleX;
    private Vector2 _friction = new Vector2(-0.1f, 0);
    private bool _isGrounded = false; 
    private bool _wasFalling = false;
    private bool _canControl = true;

    private HealthBase _healthBase;
    private Animator _currentPlayer;

    private float _horizontal;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _currentPlayer = Instantiate(playerData.player, transform);
        _myRigidbody = GetComponent<Rigidbody2D>();
        _healthBase = GetComponent<HealthBase>();
        _healthBase.OnKill += OnPlayerKill;
        _speedRun = playerData.speed.value + (playerData.speed.value * 0.5f);
        _currentScaleX = transform.localScale.x;
    }

    private void OnPlayerKill()
    {
        _healthBase.OnKill -= OnPlayerKill;
        _currentPlayer.SetTrigger(playerData.triggerDeath.value);
        _canControl = false;
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && _canControl)
        {
            HandleJump();
            HandleMovement();
            CheckGrounded();
            HandleScaleFall();
        }
    }

    private void HandleMovement()
    {
        _horizontal = Input.GetAxisRaw(playerData.moveAxis.value);

        if (Input.GetKey(playerData.run.value))
        {
            _currentSpeed = _speedRun;
            _currentPlayer.SetBool(playerData.boolSprint.value, true);
        }
        else
        {
            _currentSpeed = playerData.speed.value;
            _currentPlayer.SetBool(playerData.boolSprint.value, false);
        }

        if (_horizontal < 0)
        {
            _myRigidbody.velocity = new Vector2(-_currentSpeed, _myRigidbody.velocity.y);

            if (_myRigidbody.transform.localScale.x != -1)
            {
                _myRigidbody.transform.DOScaleX(-1,
                    playerData.playerSwipeDuration.value);
                _currentScaleX = -1;
            }

            _currentPlayer.SetBool(playerData.boolRun.value, true);
        }
        else if (_horizontal > 0)
        {
            _myRigidbody.velocity = new Vector2(_currentSpeed, _myRigidbody.velocity.y);

            if (_myRigidbody.transform.localScale.x != 1)
            {

                _myRigidbody.transform.DOScaleX(1,
                    playerData.playerSwipeDuration.value);
                _currentScaleX = 1;
            }

            _currentPlayer.SetBool(playerData.boolRun.value, true);
        }
        else
        {
            _myRigidbody.velocity = new Vector2(0, _myRigidbody.velocity.y);
            _currentPlayer.SetBool(playerData.boolRun.value, false);
        }
    }


    private void setFriction()
    {
        if (_myRigidbody.velocity.x > 0)
        {
            _myRigidbody.velocity += _friction;
        }
        else if (_myRigidbody.velocity.x < 0)
        {
            _myRigidbody.velocity -= _friction;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(playerData.jump.value) && _isGrounded)
        {
            _myRigidbody.velocity = Vector2.up * 
                playerData.jumpForce.value;

            DOTween.Kill(_myRigidbody.transform);

            _currentPlayer.SetTrigger(playerData.triggerJump.value);
            HandleScaleJump();
        }
    }


    private void HandleScaleJump()
    {
        DOTween.Kill(_myRigidbody.transform);

        _myRigidbody.transform.DOScaleY(playerData.jumpScaleY.value,
            playerData.animationDuration.value)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }


    private void HandleScaleFall()
    {
        if (_wasFalling && _isGrounded)
        {
            _wasFalling = false;
            _currentPlayer.SetBool(playerData.boolFalling.value, false); 

            DOTween.Kill(_myRigidbody.transform);

            _myRigidbody.transform.DOScale(new Vector2(playerData.fallScaleX.value 
                * _currentScaleX,
                playerData.fallScaleY.value), 
                playerData.animationDuration.value / 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _myRigidbody.transform.DOScale(new Vector2(1 * _currentScaleX, 1), 
                        playerData.animationDuration.value / 2)
                        .SetEase(Ease.OutBack);
                });
        }

        if (!_wasFalling && !_isGrounded && _myRigidbody.velocity.y < -0.1f) 
        {
            _wasFalling = true;
            _currentPlayer.SetBool(playerData.boolFalling.value, true);
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, _isGrounded ? Color.green : Color.red);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}