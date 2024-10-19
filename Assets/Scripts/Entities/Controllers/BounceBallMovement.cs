using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBallMovement : MonoBehaviour
{
    private bool isMoving = false;
    public Vector3 direction;
    public Vector3 velocity;

    private Rigidbody2D rb2d;
    private BounceBall ball;
    private BounceBallReflect reflect;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ball = GetComponent<BounceBall>();
        reflect = GetComponent<BounceBallReflect>();
    }

    private void Update()
    {
        if (reflect.reflectType == BallReflectType.DetectAndRefelct)
        {
            DetectAndMove();
        }
        else if (reflect.reflectType == BallReflectType.OnCollisionReflect)
        {
            ReflectMove();
        }
    }

    private void DetectAndMove()
    {
        direction = reflect.ReflectDirection(direction);
        transform.position += ball.Stat.CurrentBallStat.ballSpeed * direction * Time.deltaTime;
    }

    private void ReflectMove()
    {
        if (isMoving && reflect.reflectType == BallReflectType.OnCollisionReflect)
            transform.position += ball.Stat.CurrentBallStat.ballSpeed * direction * Time.deltaTime;

    }

    private void FixedUpdate()
    {
        if (isMoving && reflect.reflectType == BallReflectType.OnCollisionPhisics)
            velocity = rb2d.velocity;
    }

    public void Move(Vector3 vector)
    {
        isMoving = true;

        if (reflect.reflectType == BallReflectType.OnCollisionPhisics)
            rb2d.velocity = vector;
        else
            this.direction = vector;
    }
}
