using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
        Vector3 nextPosition = transform.position + ball.Stat.CurrentBallStat.ballSpeed * direction * Time.deltaTime;
        if (!GameManager.Instance.IsInGameArea(nextPosition))
        {
            // direction 이 왼쪽으로 가는지 오른쪽으로 가는지 확인
            if (direction.x > 0)
                direction = Quaternion.Euler(0, 0, -90) * direction;
            else
                direction = Quaternion.Euler(0, 0, 90) * direction;
            // 화면 밖으로 나가지 않도록 하기 위해 위치 보정
            nextPosition = transform.position + ball.Stat.CurrentBallStat.ballSpeed * direction * Time.deltaTime;
        }

        transform.position = nextPosition;
    }

    private void ReflectMove()
    {
        if (isMoving && reflect.reflectType == BallReflectType.OnCollisionReflect)
            transform.position += ball.Stat.CurrentBallStat.ballSpeed * direction * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isMoving && reflect.IsPhisicsReflect())
            velocity = rb2d.velocity;
    }

    public void Move(Vector3 vector)
    {
        isMoving = true;

        if (reflect.IsPhisicsReflect())
            rb2d.velocity = vector;
        else
            direction = vector.normalized;
    }
}
