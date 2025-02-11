using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [Header("Speed and Jump setup")]
    public float speed;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    [Header("Animation Setup")]
    public float jumpScaleY = 1.3f;
    public float jumpScaleX = 0.2f;
    public float fallScaleY = 0.7f;
    public float fallScaleX = 1.2f;
    public float animationDuration = 0.3f;
    public Ease ease = Ease.OutBack;

    [Header("Animation Player")]
    public string boolRun = "Run";
    public string boolSprint = "Sprint";
    public string triggerJump = "Jump";
    public string boolFalling = "Falling";
    public string triggerDeath = "Death";
    public Animator animator;
    public float playerSwipeDuration = .1f;

    [Header("Inputs Player")]
    public KeyCode moveRightKey = KeyCode.RightArrow;
    public KeyCode moveLeftKey = KeyCode.LeftArrow; 
    public KeyCode runKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;

    private Rigidbody2D _myRigidbody;
    private float _speedRun;
    private float _currentSpeed;
    private float _currentScaleX;
    private Vector2 _friction = new Vector2(-0.1f, 0);
    private bool _isGrounded = false; 
    private bool _wasFalling = false;
    private bool _canControl = true;

    private HealthBase _healthBase;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _healthBase = GetComponent<HealthBase>();
        _healthBase.OnKill += OnPlayerKill;
        _speedRun = speed + (speed * 0.5f);
        _currentScaleX = transform.localScale.x;
    }

    private void OnPlayerKill()
    {
        _healthBase.OnKill -= OnPlayerKill;
        animator.SetTrigger(triggerDeath);
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
        if (Input.GetKey(runKey))
        {
            _currentSpeed = _speedRun;
            animator.SetBool(boolSprint, true);
        }
        else
        {
            _currentSpeed = speed;
            animator.SetBool(boolSprint, false);
        }

        if (Input.GetKey(moveLeftKey))
        {
            _myRigidbody.velocity = new Vector2(-_currentSpeed, _myRigidbody.velocity.y);

            if (_myRigidbody.transform.localScale.x != -1)
            {
                _myRigidbody.transform.DOScaleX(-1, playerSwipeDuration);
                _currentScaleX = -1; 
            }

            animator.SetBool(boolRun, true);
        }
        else if (Input.GetKey(moveRightKey))
        {
            _myRigidbody.velocity = new Vector2(_currentSpeed, _myRigidbody.velocity.y);

            if (_myRigidbody.transform.localScale.x != 1)
            {

                _myRigidbody.transform.DOScaleX(1, playerSwipeDuration);
                _currentScaleX = 1; 
            }

            animator.SetBool(boolRun, true);
        }
        else
        {
            _myRigidbody.velocity = new Vector2(0, _myRigidbody.velocity.y);
            animator.SetBool(boolRun, false);
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
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _myRigidbody.velocity = Vector2.up * jumpForce;

            DOTween.Kill(_myRigidbody.transform);

            animator.SetTrigger(triggerJump);
            HandleScaleJump();
        }
    }


    private void HandleScaleJump()
    {
        DOTween.Kill(_myRigidbody.transform);

        _myRigidbody.transform.DOScaleY(jumpScaleY, animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }


    private void HandleScaleFall()
    {
        if (_wasFalling && _isGrounded)
        {
            _wasFalling = false;
            animator.SetBool(boolFalling, false); // Reseta apenas ao tocar o chão

            DOTween.Kill(_myRigidbody.transform);

            _myRigidbody.transform.DOScale(new Vector2(fallScaleX * _currentScaleX, fallScaleY), animationDuration / 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _myRigidbody.transform.DOScale(new Vector2(1 * _currentScaleX, 1), animationDuration / 2)
                        .SetEase(Ease.OutBack);
                });
        }

        // Se já está caindo, não ativa novamente
        if (!_wasFalling && !_isGrounded && _myRigidbody.velocity.y < -0.1f) // Pequeno ajuste para evitar ativações falsas
        {
            _wasFalling = true;
            animator.SetBool(boolFalling, true);
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