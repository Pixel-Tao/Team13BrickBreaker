using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildPanelDetect : MonoBehaviour
{
    //public enum dir { left, center, right }
    //public dir ddd;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ball")
    //    {
    //        BallCtrl balLCtrl = collision.gameObject.GetComponent<BallCtrl>();
    //        switch (ddd)
    //        {
    //            case dir.left:
    //                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * balLCtrl.speed;
    //                balLCtrl.UpdateCrashCount();
    //                break;
    //            case dir.center:
    //                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1).normalized * balLCtrl.speed;
    //                balLCtrl.UpdateCrashCount();
    //                break;
    //            case dir.right:
    //                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * balLCtrl.speed;
    //                balLCtrl.UpdateCrashCount();
    //                break;
    //        }
    //    }
    //}
    public float bounceAngleRange = 75f; // 공이 최대 튕겨나갈 수 있는 각도

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        // 공에 Rigidbody2D가 있는지 확인
        //Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collision.gameObject.CompareTag("Ball"))
        {
            BallCtrl ballRb = collision.gameObject.GetComponent<BallCtrl>();

            // 패들의 중앙을 기준으로 공이 충돌한 위치 계산
            Vector3 paddlePosition = transform.position;
            Vector3 contactPoint = collision.GetContact(0).point;
            float paddleWidth = GetComponent<BoxCollider2D>().bounds.size.x;

            // 충돌 지점의 상대적 위치 (-1: 왼쪽 끝, 0: 중앙, 1: 오른쪽 끝)
            float relativeHitPosition = (contactPoint.x - paddlePosition.x) / paddleWidth;

            // 충돌 위치에 따른 각도 계산
            float bounceAngle = relativeHitPosition * bounceAngleRange;

            // 반사 벡터 계산 (공의 속도를 유지하면서 각도만 변경)
            Vector2 direction = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
            direction = direction.normalized;

            // 공의 속도를 기존 속도 크기에 맞춰 반사
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = direction * ballRb.speed;
            ballRb.UpdateCrashCount();

        }
    }

}
