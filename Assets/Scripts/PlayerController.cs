using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float mouseSensitivity = 75f; 

    void Start()
    {

    }

    void Update()
    {
        
        float x = Input.GetAxis("Horizontal");
        float y = 0;

        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // 키보드와 마우스 입력을 더해서 플레이어의 위치를 업데이트
        transform.position += new Vector3(x + mouseX, y) * speed * Time.deltaTime;
    }
}

