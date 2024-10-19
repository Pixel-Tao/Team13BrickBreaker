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
    public WallType wallType = WallType.None;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (wallType == WallType.Left)
            GameManager.Instance.SetMinX(transform.position.x + (boxCollider.bounds.size.x / 2));
        else if (wallType == WallType.Right)
            GameManager.Instance.SetMaxX(transform.position.x - (boxCollider.bounds.size.x / 2));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 공의 Rigidbody2D 가져오기
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        // 공인지 확인 (태그나 다른 방법으로 공을 식별할 수 있음)
        if (rb != null && collision.gameObject.CompareTag("Ball"))
        {
            BounceBall ball = collision.gameObject.GetComponent<BounceBall>();
            ball?.WallBounce(collision, boxCollider);
        }
    }
}
