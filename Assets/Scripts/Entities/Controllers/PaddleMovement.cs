using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    private Paddle paddle;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    private Vector2 direction = Vector2.zero;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
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

        float harfWidth = boxCollider.bounds.size.x / 2;
        float maxX = GameManager.Instance.ScreenMaxX - harfWidth;
        float minX = GameManager.Instance.ScreenMinX + harfWidth;

        float nextX = transform.position.x + (movementDirection.x * paddle.speed * Time.deltaTime);

        nextX = Mathf.Clamp(nextX, minX, maxX);

        transform.position = new Vector3(nextX, transform.position.y, transform.position.z);

    }

}
