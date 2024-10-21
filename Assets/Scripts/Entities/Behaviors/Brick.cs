using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Box
{
    public event Action<PlayerType> OnBrickDestroyedEvent;
    [SerializeField][Range(0, 99)] private int hp = 5;
    public Animator brickAnimator;

    public void Start()
    {
        brickAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (ballRb != null && collision.gameObject.tag == "Ball")
        {
            BounceBall ball = collision.gameObject.GetComponent<BounceBall>();
            ball.BrickBounce(collision, boxCollider);
        }
    }

    public void OnDamaged(BounceBall ball, int value)
    {
        brickAnimator.SetTrigger("isTouch");

        GameManager.Instance.AddScrore(ball.Owner.playerType, 100);
        hp -= value;
        if (hp <= 0)
        {
            hp = 0;
            OnBrickDestroyedEvent?.Invoke(ball.Owner.playerType);
        }
    }
}
