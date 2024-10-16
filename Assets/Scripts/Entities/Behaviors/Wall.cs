using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType
{
    None,
    Top,
    Bottom,
    Left,
    Right
}

public class Wall : MonoBehaviour
{
    [SerializeField] private WallType wallType = WallType.None;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 공의 Rigidbody2D 가져오기
        Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // 공인지 확인 (태그나 다른 방법으로 공을 식별할 수 있음)
        if (ballRb != null && collision.gameObject.CompareTag("Ball"))
        {
            BounceBall ball = collision.gameObject.GetComponent<BounceBall>();
            if (wallType == WallType.Bottom)
            {
                // 공 파괴
                ball?.DestroyBall();
            }
            else
            {
                ball?.WallBounce(collision, wallType);
            }
        }
    }
}
