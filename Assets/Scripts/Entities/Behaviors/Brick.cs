using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public event Action<PlayerType> OnBrickDestroyedEvent;
    [SerializeField][Range(0, 99)] private int hp = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (ballRb != null && collision.gameObject.tag == "Ball")
        {
            BounceBall ball = collision.gameObject.GetComponent<BounceBall>();
            HpChange(ball, hp - 1);

            ball.BrickBounce(collision);
            AudioManager.Instance.PlaySfx(AudioClipType.brick_bounce);
        }
    }

    private void HpChange(BounceBall ball, int value)
    {
        GameManager.Instance.AddScrore(ball.Owner.playerType, 500);
        hp = value;
        if (hp <= 0)
        {
            OnBrickDestroyedEvent?.Invoke(ball.Owner.playerType);
        }
    }
}
