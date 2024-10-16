using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMMM : MonoBehaviour
{
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) { transform.Translate(-1 * speed * Time.deltaTime, 0, 0); }
        if (Input.GetKey(KeyCode.D)) { transform.Translate(1 * speed * Time.deltaTime, 0, 0); }
    }
}
