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
    [SerializeField] private GameObject fadeUIPrefab;

    private GameUI gameUI;
    private FadeUI fadeUI;

    void Start()
    {
        // Scene 진입점
        fadeUI = Instantiate(fadeUIPrefab).GetComponent<FadeUI>();
        gameUI = Instantiate(gameUIPrefab).GetComponent<GameUI>();
        Instantiate(wallPrefab);
        Instantiate(brickAreaPrefab);

        UIManager.Instance.OnFadeEvent -= Fade;
        UIManager.Instance.OnFadeEvent += Fade;
        GameManager.Instance.OnPlayerJoinEvent += PlayerJoin;
        GameManager.Instance.OnBallGenerateEvent += BallGenerate;

        UIManager.Instance.FadeIn(() =>
        {
            GameManager.Instance.GameStart();
        });
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

    private void Fade(FadeType fadeType, System.Action fadedAction)
    {
        fadeUI.Play(fadeType, fadedAction);
    }
}