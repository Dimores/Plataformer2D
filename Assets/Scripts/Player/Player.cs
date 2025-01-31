using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [Header("Speed and Jump setup")]
    public float speed;
    public float jumpForce = 5f;

    [Header("Animation setup")]
    public float jumpScaleY = 1.3f;
    public float jumpScaleX = 0.2f;
    public float fallScaleY = 0.7f;
    public float fallScaleX = 1.2f;
    public float animationDuration = 0.3f;
    public Ease ease = Ease.OutBack;

    private Rigidbody2D _myRigidbody;
    private float _speedRun;
    private float _currentSpeed;
    private Vector2 _friction = new Vector2(-0.1f, 0);
    private bool _isGrounded = false; 
    private bool _wasFalling = false; 

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _speedRun = speed + (speed * 0.5f); 
    }

    void Update()
    {
        HandleJump();
        HandleMovement();
        CheckGrounded();
        HandleScaleFall();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            _currentSpeed = _speedRun;
        else
            _currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _myRigidbody.velocity = new Vector2(-_currentSpeed, _myRigidbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _myRigidbody.velocity = new Vector2(_currentSpeed, _myRigidbody.velocity.y);
        }

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
            _myRigidbody.transform.localScale = Vector2.one;

            DOTween.Kill(_myRigidbody.transform); 

            HandleScaleJump();
        }
    }

    private void HandleScaleJump()
    {
        _myRigidbody.transform.DOScaleY(jumpScaleY, animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(ease);

        _myRigidbody.transform.DOScaleX(jumpScaleX, animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(ease);
    }

    private void HandleScaleFall()
    {
        if (_wasFalling && _isGrounded)
        {
            _wasFalling = false; 

            DOTween.Kill(_myRigidbody.transform); 

            _myRigidbody.transform.DOScale(new Vector2(fallScaleX, fallScaleY), animationDuration / 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _myRigidbody.transform.DOScale(Vector2.one, animationDuration / 2).SetEase(Ease.OutBack);
                });
        }

        if (_myRigidbody.velocity.y < 0 && !_isGrounded)
        {
            _wasFalling = true; 
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
