using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCCC : MonoBehaviour
{
    public int hp = 5;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball") 
        {
            hp -= 1;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
