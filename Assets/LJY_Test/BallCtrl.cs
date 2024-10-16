using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCtrl : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 dir;

    public int crashCount = 0;


    Rigidbody2D rb2d;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(-10, -10).normalized * speed;
    }

    void FixedUpdate()
    {
        if (rb2d.velocity != Vector2.zero)
        {
            dir = rb2d.velocity;
        }
    }

    public void UpdateCrashCount()
    {
        crashCount += 1;
        speed += 0.1f;
    }
}
