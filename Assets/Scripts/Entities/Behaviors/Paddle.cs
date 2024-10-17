using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<float> OnLookEvent;
    public event Action OnShootEvent;
    public event Action OnPaddleDestoryEvent;

    AudioSource audioSource; // 오디오 추가 
    public AudioClip clip; // 오디오 추가

    public PlayerType playerType;
    [Range(1, 20)] public float speed;

    public float bounceAngleRange = 75f; // 공이 최대 튕겨나갈 수 있는 각도
    private float[] arrAngles = { -30, -45, -60, 60, 45, 30 }; //발사시, 무작위로 발사될 각도 값의 배열
    private HashSet<BounceBall> myBalls = new HashSet<BounceBall>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  // 오디오 추가
        myBalls.Clear();
        GameManager.Instance.BallGenerate(this);
    }

    private void Update()
    {
        CheckAliveAndResurrect();
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
        audioSource.PlayOneShot(clip); // shot을 누를 때 소리가 나게
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            BounceBall ballRb = collision.gameObject.GetComponent<BounceBall>();
            ballRb.PaddleBounce(collision, this);
        }
    }
}
