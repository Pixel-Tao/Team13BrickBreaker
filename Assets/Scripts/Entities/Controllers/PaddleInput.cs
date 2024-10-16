using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleInput : MonoBehaviour
{
    private Paddle paddle;
    private Camera camera;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
        camera = Camera.main;
    }

    #region Player1 키 이벤트
    public void OnKeyMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Debug.Log(input);
        paddle.CallMove(input);
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
        dir.x = dir.x > 0 ? 1 : -1;
        paddle.CallMove(dir);
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
