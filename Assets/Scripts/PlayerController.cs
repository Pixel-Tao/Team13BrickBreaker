using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    private float[] arrAngles = { -30, -45, -60, 60, 45, 30 }; //발사시, 무작위로 발사될 각도 값의 배열

    public float speed;
    public float mouseSensitivity = 75f;

    private bool isFirstShot = true;

    private const float rotateValue = 180f; //180도 고정값

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 temp = collision.transform.eulerAngles; //부딪힌 볼에 대한 각도를 가져온다.

        //공 닿으면 랜덤으로 튕기는 로직
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (isFirstShot == true)
            {
                int random = Random.Range(0, 6);
                temp.z = arrAngles[random]; //랜덤 값이 반영된다.
                collision.transform.eulerAngles = temp; //각도 적용
                isFirstShot = false;
            }
            else
            {
                temp.z = rotateValue + temp.z; //z값에 180 - (현재 볼의 각도)
                collision.transform.eulerAngles = temp;
            }
        }
    }
}

