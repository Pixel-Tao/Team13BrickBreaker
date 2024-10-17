using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPaddle(collision.transform);
    }

    void CheckPaddle(Transform other)
    {
        Debug.Log("CheckPaddle");
    }

}
