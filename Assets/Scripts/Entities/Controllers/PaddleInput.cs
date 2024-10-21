using Newtonsoft.Json.Linq;
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

    private void Update()
    {
        ApplyCallMove();
    }

    private void ApplyCallMove()
    {
        Vector2 dir = direction != Vector2.zero ? direction : Vector2.zero;
        paddle.CallMove(dir);
        paddle.CallLook(dir.x);
    }

    private bool AllowInputEvent()
    {
        return UIManager.Instance.IsPause == false;
    }

    #region Player1 키 이벤트
    public void OnKeyMove(InputValue value)
    {
        if (!AllowInputEvent()) return;

        direction = value.Get<Vector2>();
    }

    public void OnKeyShoot(InputValue value)
    {
        if (!AllowInputEvent()) return;

        if (value.isPressed)
        {
            paddle.CallShoot();
        }
    }
    public void OnKeyEscape(InputValue value)
    {
        if (!AllowInputEvent()) return;

        if (value.isPressed)
        {
            UIManager.Instance.Pause();
        }
    }
    #endregion

    #region Player2 키 이벤트
    public void OnMouseMove(InputValue value)
    {
        if (!AllowInputEvent()) return;

        Vector2 screenPos = value.Get<Vector2>();
        Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
        Vector2 dir = (worldPos - transform.position).normalized;
        dir.y = 0;
        // 마우스 커서를 움직이지 않으면 이벤트가 발생하지 않음.
        // 그래서 한쪽으로 계속 가버리는 문제가 있음.
        if (dir.x < 0.05f && dir.x > -0.05f)
            dir.x = 0;
        else
            dir.x = dir.x > 0 ? 1 : -1;

        direction = dir;
    }

    public void OnMouseShoot(InputValue value)
    {
        if (!AllowInputEvent()) return;

        if (value.isPressed)
        {
            paddle.CallShoot();
        }
    }

    public void OnMouseEscape(InputValue value)
    {
        if (!AllowInputEvent()) return;

        if (value.isPressed)
        {
            UIManager.Instance.Pause();
        }
    }
    #endregion
}
