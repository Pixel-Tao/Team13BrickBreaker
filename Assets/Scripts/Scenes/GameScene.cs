using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    #region Prefabs
    [SerializeField] private GameObject gameUIPrefab;
    [SerializeField] private GameObject bounceBallPrefab;
    [SerializeField] private GameObject paddle1Prefab;
    [SerializeField] private GameObject paddle2Prefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject brickAreaPrefab;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject gameoverUIPrefab;
    [SerializeField] private GameObject fadeUIPrefab;

    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private StageSO stageSO;

    private GameUI gameUI;
    private FadeUI fadeUI;
    #endregion

    void Start()
    {
        // GameScene 진입점
        // UI 초기화
        InitUI();
        // Event 초기화
        InitEvent();
        // 게임 정보 초기화
        GameManager.Instance.InitGame(itemPrefabs.Count);
        // 스테이지 정보 초기화
        StageManager.Instance.InitStage(stageSO);
        // 플레이어 데이터 리셋
        GameManager.Instance.PlayerReset();
        // 게임 스테이지 불러오기 -> 게임 시작됨
        StageManager.Instance.LoadStage();
        // 사운드 플레이
        AudioManager.Instance.GamePlay();
        // 화면을 점차적으로 밝게 한다.
        UIManager.Instance.FadeIn();
    }

    private void InitUI()
    {
        fadeUI = Instantiate(fadeUIPrefab).GetComponent<FadeUI>();
        gameUI = Instantiate(gameUIPrefab).GetComponent<GameUI>();
        Instantiate(wallPrefab);

    }

    private void InitEvent()
    {
        StageManager.Instance?.ClearEvent();
        UIManager.Instance?.ClearEvent();
        GameManager.Instance?.ClearEvent();

        UIManager.Instance.OnFadeEvent += Fade;
        GameManager.Instance.OnPlayerJoinEvent += PlayerJoin;
        GameManager.Instance.OnBallGenerateEvent += BallGenerate;
        GameManager.Instance.OnItemDropEvent += ItemDrop;
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


    private void BallGenerate(Paddle owner, Vector3? position = null)
    {
        GameObject ball = Instantiate(bounceBallPrefab);
        if (position.HasValue)
            // 아이템으로 인해서 생성 되는 공의 위치를 조정하기 위한 함수
            ball.GetComponent<BounceBall>().SetInfo(owner, position.Value);
        else
            // Owner 기준으로 공이 생성되도록 하는 함수
            ball.GetComponent<BounceBall>().SetInfo(owner);
    }

    private void ItemDrop(Vector3 pos, int index)
    {
        GameObject item = Instantiate(itemPrefabs[index]);
        item.transform.position = pos;
    }

    private void Fade(FadeType fadeType, System.Action fadedAction)
    {
        fadeUI.Play(fadeType, fadedAction);
    }
}