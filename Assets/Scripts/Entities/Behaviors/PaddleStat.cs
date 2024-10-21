using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleStat : MonoBehaviour
{
    public event Action<Item> OnItemEffectEvent;
    public PaddleStatData BallDefaultStat;
    private Paddle paddle;
    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    public void ApplyItemEffect(Item item)
    {
        // - paddle이 하는 행위
        //  1. 효과를 적용한다.
        if (item.ItemType == ItemType.BallMultipleAdd)
        {
            // 공 분신술인 경우 첫번째 공만 가져와서 분신술 적용
            BallMultipleAddItem ballMultipleAddItem = item as BallMultipleAddItem;
            BounceBall ball = paddle.GetFirstBall();
            ball?.GenerateBall(ballMultipleAddItem.ballAddCountValue);
        }
        else
        {
            // 그게 아니면 다 공의 수치를 조정
            OnItemEffectEvent?.Invoke(item);
        }
    }

    private void OnDestroy()
    {
        OnItemEffectEvent = null;
    }
}
