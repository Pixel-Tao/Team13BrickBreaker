using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Item_BallPowerUp : Item
{
    BounceBall Ball; //BounceBall.cs 에서 Action으로 UseItemBallPowerUp 꺼내올 수 있는 상태임

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPlayer(collision.transform);
    }

    void CheckPlayer(Transform other)
    {
        if (other.CompareTag("Paddle"))
        {
            Ball.UseItemBallPowerUp();
            Destroy(this);
        }
    }
}
