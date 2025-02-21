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

    [Header("VFX")]
    public Vector3 walkVFXOffset;
    public Vector3 jumpVFXOffset;
    public Vector3 fallVFXOffset;

    private Rigidbody2D _rb;
    private float _speedRun;
    private float _currentSpeed;
    private float _currentScaleX;
    private bool _isGrounded = false; 
    private bool _wasFalling = false;
    private bool _canControl = true;

    private HealthBase _healthBase;
    private Animator _currentPlayer;

    [SerializeField] private float _horizontal;

    public float Horizontal { get => _horizontal; set => _horizontal = value; }
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public bool WasFalling { get => _wasFalling; set => _wasFalling = value; }
    public bool CanControl { get => _canControl; set => _canControl = value; }
    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public float SpeedRun { get => _speedRun; set => _speedRun = value; }
    public Rigidbody2D Rb { get => _rb; set => _rb = value; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        CheckGrounded();
        if (gameObject.activeInHierarchy && _canControl)
        {
            HandleJump();
            HandleMovement();
            HandleScaleFall();
        }
    }

    private void HandleMovement()
    {
        _horizontal = Input.GetAxisRaw(playerData.moveAxis.value);

        if (Input.GetKey(playerData.run.value))
        {
            _currentSpeed = _speedRun;
        }
        else
        {
            _currentSpeed = playerData.speed.value;
        }

        if (Horizontal < 0)
        {
            _rb.velocity = new Vector2(-CurrentSpeed, _rb.velocity.y);

            if (_rb.transform.localScale.x != -1)
            {
                _rb.transform.DOScaleX(-1,
                    playerData.playerSwipeDuration.value);
                _currentScaleX = -1;
            }

        }
        else if (Horizontal > 0)
        {
            _rb.velocity = new Vector2(CurrentSpeed, _rb.velocity.y);

            if (_rb.transform.localScale.x != 1)
            {

                _rb.transform.DOScaleX(1,
                    playerData.playerSwipeDuration.value);
                _currentScaleX = 1;
            }

        }
        else
        {
            _rb.velocity = new Vector2(0, Rb.velocity.y);
        }
    }

    public void DisableControl(float duration)
    {
        CanControl = false;
        Invoke(nameof(EnableControl), duration);
    }

    private void EnableControl()
    {
        CanControl = true;
    }

    private void PlayJumpVFX()
    {
        VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position, jumpVFXOffset);
    }

    private void PlayFallVFX()
    {
        VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.FALL, transform.position, fallVFXOffset);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(playerData.jump.value) && _isGrounded)
        {
            _rb.velocity = Vector2.up * 
                playerData.jumpForce.value;

            DOTween.Kill(_rb.transform);

            _rb.transform.localScale = new Vector3(_currentScaleX, 1, 1);

            HandleScaleJump();
            PlayJumpVFX();
        }
    }


    private void HandleScaleJump()
    {
        DOTween.Kill(Rb.transform);

        _rb.transform.DOScaleY(playerData.jumpScaleY.value,
            playerData.animationDuration.value)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }


    private void HandleScaleFall()
    {
        if (_wasFalling && _isGrounded)
        {
            _wasFalling = false;

            DOTween.Kill(Rb.transform);

            _rb.transform.DOScale(new Vector2(playerData.fallScaleX.value 
                * _currentScaleX,
                playerData.fallScaleY.value), 
                playerData.animationDuration.value / 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _rb.transform.DOScale(new Vector2(1 * _currentScaleX, 1), 
                        playerData.animationDuration.value / 2)
                        .SetEase(Ease.OutBack);
                });
            PlayFallVFX();
        }

        if (!_wasFalling && !_isGrounded && _rb.velocity.y < -0.1f) 
        {
            _wasFalling = true;
        }
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}