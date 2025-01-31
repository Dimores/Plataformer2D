using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigdbody;

    public Vector2 velocity;

    public float speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigdbody.velocity = new Vector2(-speed, myRigdbody.velocity.y);
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigdbody.velocity = new Vector2(speed, myRigdbody.velocity.y);
        }
    }
}
