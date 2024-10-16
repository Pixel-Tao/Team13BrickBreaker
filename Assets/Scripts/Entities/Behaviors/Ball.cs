using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //TODO: Ball 사용 전 Wall 오브젝트에 각각 태그 걸어야함. (상위: TopWall, 좌우: Wall)

    private float ballSpeed = 10f; 
    private const float rotateValue = 180f; //180도 고정값


    private void Update()
    {
        float applySpeed = Time.deltaTime * ballSpeed;
        transform.Translate(new Vector2(0, applySpeed)); //vector값을 갖고와서 speed넣어서 쭉 보내버리는 함수 // 매시간마다(time.deltaTime) ballSpeed만큼 앞으로 갈것.
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 temp = transform.eulerAngles; //볼의 각도를 가져와서 temp에 담아줌

        if (other.gameObject.CompareTag("TopWall"))
        {
            temp.z = rotateValue - temp.z; //z값에 180 - (현재 볼의 각도)
            transform.eulerAngles = temp;
        }

        else if (other.gameObject.CompareTag("Wall"))
        {
            temp.z = (rotateValue * 2) - temp.z; //위로 갈수도 아래로갈수도 있기에 360'로 만들고 - temp.z
            transform.eulerAngles = temp;
        }
    }
}