using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleInput : MonoBehaviour
{
    private Paddle paddle;
    private Camera camera;

    private Vector2 direction = Vector2.zero;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
        camera = Camera.main;
    }
    #region Player1 키 이벤트
    public void OnKeyMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        paddle.CallMove(direction);
        paddle.CallLook(direction.x);
    }

    public void OnKeyShoot(InputValue value)
    {
        if (value.isPressed)
        {
            paddle.CallShoot();
        }
    }
    #endregion

    #region Player2 키 이벤트
    public void OnMouseMove(InputValue value)
    {
        Vector2 screenPos = value.Get<Vector2>();
        Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
        Vector2 dir = (worldPos - transform.position).normalized;
        dir.y = 0;
        // 마우스 커서를 움직이지 않으면 이벤트가 발생하지 않음.
        // 그래서 한쪽으로 계속 가버리는 문제가 있음.
        if (dir.x < 0.02f && dir.x > -0.02f)
            dir.x = 0;
        else
            dir.x = dir.x > 0 ? 1 : -1;

        direction = dir;
        paddle.CallMove(direction);
        paddle.CallLook(direction.x);
    }

    public void OnMouseShoot(InputValue value)
    {
        if (value.isPressed)
        {
            paddle.CallShoot();
        }
    }
    #endregion
}
