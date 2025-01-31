using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigdbody;
    public float speed;
    public float jumpForce = 5f;

    private float _speedRun;
    private float _currentSpeed;
    private Vector2 _friction = new Vector2(-.1f, 0);

    private void Start()
    {
        // 50% More faster than walking
        _speedRun = speed + (speed * 0.5f);
    }

    void Update()
    {
        HandleJump();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            _currentSpeed = _speedRun;
        else
            _currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigdbody.velocity = new Vector2(-_currentSpeed, myRigdbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigdbody.velocity = new Vector2(_currentSpeed, myRigdbody.velocity.y);
        }

        if (myRigdbody.velocity.x > 0)
        {
            myRigdbody.velocity += _friction;
        } else if (myRigdbody.velocity.x < 0)
        {
            myRigdbody.velocity -= _friction;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            myRigdbody.velocity = Vector2.up * jumpForce;
    }

}
