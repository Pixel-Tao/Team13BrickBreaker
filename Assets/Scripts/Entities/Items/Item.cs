using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  ItemType
{
    None,
    BallPowerUp,
    BallMultipleAdd,
    BallSpeedDown,
}

public abstract class Item : MonoBehaviour
{
    // 아이템 타입
    public abstract ItemType ItemType { get; protected set; }

    // 아이템을 파괴하는 함수
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    // 아이템이 Padddle에 닿았을 때 OR 아래 벽에 닿았을때의 처리
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 아이템이 플레이어에게 효과를 주는 방향
        if (collision.CompareTag("Paddle"))
        {
            // 플레이어게 효과를 줌.
            collision.GetComponent<PaddleStat>().ApplyItemEffect(this);
            // 사라짐
            DestroyItem();
        }
        else if(collision.CompareTag("BottomWall"))
        {
            // 바닥에 닿으면 사라짐
            DestroyItem();
        }
    }
}
