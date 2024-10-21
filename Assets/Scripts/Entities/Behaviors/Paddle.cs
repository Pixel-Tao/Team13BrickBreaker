using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Paddle : Box
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<float> OnLookEvent;
    public event Action OnShootEvent;
    public event Action OnPaddleDestoryEvent;

    public PaddleStat Stat { get; private set; }

    public PlayerType playerType;
    [Range(1, 20)] public float speed;

    public float bounceAngleRange = 75f; // 공이 최대 튕겨나갈 수 있는 각도
    private float[] arrAngles = { -30, -45, -60, 60, 45, 30 }; //발사시, 무작위로 발사될 각도 값의 배열
    private HashSet<BounceBall> myBalls = new HashSet<BounceBall>();

    protected override void Awake()
    {
        base.Awake();
        Stat = GetComponent<PaddleStat>();
        GameManager.Instance.OnPlayerClearEvent += StageClear;
    }
    private void Start()
    {
        myBalls.Clear();
        GameManager.Instance.BallGenerate(this);
    }

    private void Update()
    {
        CheckAliveAndResurrect();
    }

    private void OnDestroy()
    {
        OnMoveEvent = null;
        OnLookEvent = null;
        OnShootEvent = null;
        OnPaddleDestoryEvent = null;
        GameManager.Instance.OnPlayerClearEvent -= StageClear;
    }

    private void CheckAliveAndResurrect()
    {
        // 소유한 공이 없으면 부활 시도
        if (myBalls.Count < 1)
        {
            ResurrectOrDead();
        }
    }

    private void ResurrectOrDead()
    {
        if (GameManager.Instance.HasLife(playerType))
        {
            // Life가 남아있으면 부활
            GameManager.Instance.DecreaseLife(playerType);
            GameManager.Instance.BallGenerate(this);
        }
        else
        {
            // Life 가 없으면 사망
            OnPaddleDestoryEvent?.Invoke();
        }
    }

    public void CallMove(Vector2 vector2)
    {
        OnMoveEvent?.Invoke(vector2);
    }

    public void CallLook(float directionX)
    {
        // 1. 바라 볼 대상
        // directionX 왼쪽 = -1, 1 = 오른쪽
        OnLookEvent?.Invoke(directionX);
    }

    public void CallShoot()
    {
        OnShootEvent?.Invoke();
    }

    public void AddMyBall(BounceBall ball)
    {
        if (!myBalls.Contains(ball))
            myBalls.Add(ball);
    }

    public void RemoveMyBall(BounceBall ball)
    {
        if (myBalls.Contains(ball))
            myBalls.Remove(ball);
    }

    public BounceBall GetFirstBall()
    {
        return myBalls.FirstOrDefault();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            BounceBall ball = collision.gameObject.GetComponent<BounceBall>();
            ball.PaddleBounce(collision, this);
        }
    }

    private void StageClear()
    {
        for (int i = myBalls.Count - 1; i >= 0; i--)
        {
            BounceBall ball = myBalls.ElementAt(i);
            ball.DestroyBall();
            RemoveMyBall(ball);
        }
        Destroy(gameObject);
    }
}
