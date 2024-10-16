using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallReflect : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 공의 Rigidbody2D 가져오기
        Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // 공인지 확인 (태그나 다른 방법으로 공을 식별할 수 있음)
        if (ballRb != null && collision.gameObject.CompareTag("Ball"))
        {
            BallCtrl balLCtrl = collision.gameObject.GetComponent<BallCtrl>();

            Debug.Log("입장~");
            // 충돌한 표면의 법선 벡터
            Vector2 normal = collision.contacts[0].normal * -1;
            Debug.Log(normal);
            // 공의 현재 이동 벡터 (입사 벡터)
            Vector2 incomingVector = balLCtrl.dir;
            Debug.Log(incomingVector);
            // 반사 벡터 계산
            Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);
            Debug.Log(reflectVector);

            reflectVector = reflectVector.normalized * balLCtrl.speed;

            // 반사 벡터로 공의 속도 변경
            ballRb.velocity = reflectVector;

            balLCtrl.UpdateCrashCount();
        }
    }
}
