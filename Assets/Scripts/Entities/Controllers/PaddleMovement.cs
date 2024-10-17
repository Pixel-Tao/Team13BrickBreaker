using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    private Paddle paddle;
    private Rigidbody2D rigidbody;

    private Vector2 direction = Vector2.zero;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        paddle.OnMoveEvent += Move;
    }

    private void FixedUpdate()
    {
        ApplyMovement(direction);
    }

    private void Move(Vector2 direction)
    {
        this.direction = direction;
    }

    private void ApplyMovement(Vector2 movementDirection)
    {
        if (direction.x == 0) return;

        rigidbody.velocity = movementDirection * paddle.speed;
    }

}
