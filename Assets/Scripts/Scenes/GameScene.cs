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
    [SerializeField] private GameObject fadeUIPrefab;

    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private StageLevelSO stageLevelSO;

    private GameUI gameUI;
    private Stage currentStage;

    private FadeUI fadeUI;

    void Start()
    {
        // Scene 진입점
        fadeUI = Instantiate(fadeUIPrefab).GetComponent<FadeUI>();
        gameUI = Instantiate(gameUIPrefab).GetComponent<GameUI>();
        Instantiate(wallPrefab);

        UIManager.Instance.SetEvent(Fade);

        GameManager.Instance.OnPlayerJoinEvent -= PlayerJoin;
        GameManager.Instance.OnPlayerJoinEvent += PlayerJoin;
        GameManager.Instance.OnBallGenerateEvent -= BallGenerate;
        GameManager.Instance.OnBallGenerateEvent += BallGenerate;
        GameManager.Instance.OnStageLoadEvent -= LoadStage;
        GameManager.Instance.OnStageLoadEvent += LoadStage;
        GameManager.Instance.OnItemDropEvent -= ItemDrop;
        GameManager.Instance.OnItemDropEvent += ItemDrop;

        GameManager.Instance.LoadStage(itemPrefabs.Count);

        UIManager.Instance.FadeIn(() =>
        {
            GameManager.Instance.GameStart();

            //AudioManager.Instance.PlayBgm(AudioClipType.bgm1);
            //AudioManager.Instance.VolumeBgm(0.1f);
            AudioManager.Instance.VolumeSfx(0.1f);
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

    private void Fade(FadeType fadeType, System.Action fadedAction)
    {
        fadeUI.Play(fadeType, fadedAction);
    }
}