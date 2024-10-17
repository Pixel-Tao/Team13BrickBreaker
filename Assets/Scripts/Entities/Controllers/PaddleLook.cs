using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleLook : MonoBehaviour
{
    private Paddle paddle;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        paddle.OnLookEvent += Look;
    }

    private void Look(float directionX)
    {
        // 2. 바라보는 방법
        if (directionX == 0) return;

        ApplyLook(directionX < 0);
    }

    private void ApplyLook(bool isLeft)
    {
        // 3. 바라보게 하는거..
        spriteRenderer.flipX = isLeft;
    }
}
