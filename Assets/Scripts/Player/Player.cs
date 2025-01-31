using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigdbody;

    public float speed;

    public float jumpForce = 5f;

    private Vector2 _friction = new Vector2(-.1f, 0);

    void Update()
    {
        HandleJump();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigdbody.velocity = new Vector2(-speed, myRigdbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigdbody.velocity = new Vector2(speed, myRigdbody.velocity.y);
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
