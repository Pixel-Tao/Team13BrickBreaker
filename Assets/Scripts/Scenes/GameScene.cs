using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject gameUIPrefab;
    [SerializeField] private GameObject bounceBallPrefab;
    [SerializeField] private GameObject paddle1Prefab;
    [SerializeField] private GameObject paddle2Prefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject brickAreaPrefab;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject gameoverUIPrefab;

    private GameUI gameUI;
<<<<<<< HEAD
    
=======
    private GameOverUI gameOverUI;

>>>>>>> dev
    void Start()
    {
        // Scene 진입점
        gameUI = Instantiate(gameUIPrefab).GetComponent<GameUI>();
        gameOverUI = Instantiate(gameoverUIPrefab).GetComponent<GameOverUI>();
        gameOverUI.gameObject.SetActive(false);
        Instantiate(wallPrefab);
        Instantiate(brickAreaPrefab);

        GameManager.Instance.OnPlayerJoinEvent += PlayerJoin;
        GameManager.Instance.OnBallGenerateEvent += BallGenerate;
        GameManager.Instance.OnGameOverEvent += GameOver;
        GameManager.Instance.GameStart();
    }

    private void PlayerJoin(PlayerType playerType)
    {
        if (playerType == PlayerType.Player1)
        {
            Instantiate(paddle1Prefab);
        }
        else if (playerType == PlayerType.Player2)
        {
            Instantiate(paddle2Prefab);
        }

        if (gameUI != null)
            gameUI.PlayerJoin(playerType);
    }

    private void BallGenerate(Paddle owner)
    {
        GameObject ball = Instantiate(bounceBallPrefab);
        ball.GetComponent<BounceBall>().SetInfo(owner);
    }

    public void GameOver()
    {
        gameOverUI.gameObject.SetActive(true);
    }
}