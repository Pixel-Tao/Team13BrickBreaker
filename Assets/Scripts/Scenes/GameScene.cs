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
    [SerializeField] private GameObject AudioManager;
    [SerializeField] private GameObject gameoverUIPrefab;

    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private StageLevelSO stageLevelSO;

    private GameUI gameUI;
    private Stage currentStage;
    
    void Start()
    {
        // Scene 진입점
        gameUI = Instantiate(gameUIPrefab).GetComponent<GameUI>();
        Instantiate(wallPrefab);

        GameManager.Instance.OnPlayerJoinEvent -= PlayerJoin;
        GameManager.Instance.OnPlayerJoinEvent += PlayerJoin;
        GameManager.Instance.OnBallGenerateEvent -= BallGenerate;
        GameManager.Instance.OnBallGenerateEvent += BallGenerate;
        GameManager.Instance.OnStageLoadEvent -= LoadStage;
        GameManager.Instance.OnStageLoadEvent += LoadStage;
        GameManager.Instance.OnItemDropEvent -= ItemDrop;
        GameManager.Instance.OnItemDropEvent += ItemDrop;

        GameManager.Instance.LoadStage(itemPrefabs.Count);
        GameManager.Instance.GameStart();

        //GameManager.Instance.audioManagerPrefab = AudioManager;
        //GameManager.Instance.CreateAudio();

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

    private void LoadStage(int level)
    {
        GameObject stagePrefab = stageLevelSO.stages[level - 1];
        currentStage = Instantiate(stagePrefab).GetComponent<Stage>();
    }

    private void ItemDrop(Vector3 pos, int index)
    {
        GameObject item = Instantiate(itemPrefabs[index]);
        item.transform.position = pos;
    }
}