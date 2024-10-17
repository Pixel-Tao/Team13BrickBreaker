using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemBallPowerUp : Item
{
    GameObject ball;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ball = GameObject.Find("BounceBall(Clone)");

        if (collision.transform.CompareTag("Paddle"))
        {
            ball.GetComponent<BounceBall>().UseItemBallPowerUp();
            Destroy(this);
        }
    }
}
