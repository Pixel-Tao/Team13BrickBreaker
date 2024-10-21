using UnityEngine;

public class PaddleDestory : MonoBehaviour
{
    private Paddle paddle;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void Start()
    {
        paddle.OnPaddleDestoryEvent += DestroyPaddle;
    }

    public void DestroyPaddle()
    {
        GameManager.Instance.Dead(paddle.playerType);
        Destroy(gameObject);

        GameManager.Instance.TryGameOver();
    }
}
