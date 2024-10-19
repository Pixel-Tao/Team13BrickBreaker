using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private readonly int DEFAULT_LIFE = 3;
    private readonly float DEFAULT_ITEM_DROP_RATE = 0.5f;

    private static GameManager _instance;
    public static GameManager Instance { get { Init(); return _instance; } }

    private int selectedLevel = 2;
    private int dropableItemCount = 0;

    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    private static void Init()
    {
        if (_instance == null)
        {
            // GameManager 동적 생성
            GameObject go = new GameObject { name = "GameManager" };
            _instance = go.AddComponent<GameManager>();
            DontDestroyOnLoad(go);
        }
    }

    public event Action<PlayerType> OnPlayerJoinEvent;
    public event Action<Paddle, Vector3?> OnBallGenerateEvent;
    public event Action<PlayerType, int> OnLifeChangedEvent;
    public event Action<PlayerType, int> OnScoreChangedEvent;
    public event Action<int> OnStageLoadEvent;
    public event Action<Vector3, int> OnItemDropEvent;

    private Dictionary<PlayerType, PlayerData> players;

    /// <summary>
    /// 게임 데이터 초기화
    /// </summary>
    private void GameReset()
    {
        if (players == null) players = new Dictionary<PlayerType, PlayerData>();
        else players.Clear();

        for (int i = 0; i < Enum.GetValues(typeof(PlayerType)).Length; i++)
        {
            PlayerType playerType = (PlayerType)i;
            if (playerType == PlayerType.None) continue;
            PlayerData playerData = new PlayerData();
            playerData.type = playerType;
            playerData.life = 0;
            playerData.score = 0;
            playerData.isDead = true;
            players.Add(playerType, playerData);
        }
    }

    public void LoadStage(int dropableItemCount)
    {
        this.dropableItemCount = dropableItemCount;
        OnStageLoadEvent?.Invoke(selectedLevel);
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        GameReset();

        PlayerJoin(PlayerType.Player1);
        //PlayerJoin(PlayerType.Player2);

    }

    /// <summary>
    /// 공 생성
    /// </summary>
    /// <param name="owner"></param>
    public void BallGenerate(Paddle owner, Vector3? position = null)
    {
        OnBallGenerateEvent?.Invoke(owner, position);
    }

    /// <summary>
    /// 점수 value만큼 추가
    /// </summary>
    /// <param name="type">플레이어</param>
    /// <param name="value"></param>
    public void AddScrore(PlayerType type, int value)
    {
        PlayerData playerData = players[type];
        playerData.score += value;
        players[type] = playerData;
        OnScoreChangedEvent?.Invoke(type, playerData.score);
    }

    /// <summary>
    /// 현재 점수 가져오기
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetScore(PlayerType type)
    {
        return players[type].score;
    }

    /// <summary>
    /// 생명력 1감소
    /// </summary>
    /// <param name="type"></param>
    public void DecreaseLife(PlayerType type)
    {
        PlayerData playerData = players[type];
        playerData.life--;
        players[type] = playerData;
        OnLifeChangedEvent?.Invoke(type, playerData.life);
    }

    /// <summary>
    /// 생명력 value만큼 추가
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public void AddLife(PlayerType type, int value)
    {
        PlayerData playerData = players[type];
        playerData.life += value;
        players[type] = playerData;
        OnLifeChangedEvent?.Invoke(type, playerData.life);
    }

    /// <summary>
    /// 생명력 가져오기
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetLife(PlayerType type)
    {
        return players[type].life;
    }

    /// <summary>
    /// 플레이어 목숨이 있는지 확인
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool HasLife(PlayerType type)
    {
        PlayerData player = players[type];
        return player.life > 0 && player.isDead == false;
    }

    /// <summary>
    /// 플레이어 사망처리
    /// </summary>
    /// <param name="type"></param>
    public void Dead(PlayerType type)
    {
        PlayerData playerData = players[type];
        playerData.isDead = true;
        players[type] = playerData;
    }

    /// <summary>
    /// 플레이어가 살아 있는지
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsAlive(PlayerType type)
    {
        return players[type].isDead == false;
    }

    /// <summary>
    /// 플레이어가 살아 있는지 죽었는지 확인하고 다 죽었으면 게임 오버
    /// </summary>
    public void TryGameOver()
    {
        if (IsAlive(PlayerType.Player1) || IsAlive(PlayerType.Player2)) return;

        // TODO : 게임 오버 처리
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// 플레이어 합류
    /// </summary>
    /// <param name="player"></param>
    public void PlayerJoin(PlayerType player)
    {
        PlayerData playerData = players[player];
        playerData.life = DEFAULT_LIFE;
        playerData.isDead = false;
        players[player] = playerData;

        // 플레이어 추가.
        OnPlayerJoinEvent?.Invoke(player);
    }

    public void DropItem(Vector3 pos)
    {
        if (UnityEngine.Random.Range(0f, 1f) <= DEFAULT_ITEM_DROP_RATE) return;
        OnItemDropEvent?.Invoke(pos, UnityEngine.Random.Range(0, dropableItemCount));
    }

    public void SetMinX(float x)
    {
        this.MinX = x;
    }

    public void SetMaxX(float x)
    {
        this.MaxX = x;
    }

    public void SetMinY(float y)
    {
        this.MinY = y;
    }

    public void SetMaxY(float y)
    {
        this.MaxY = y;
    }

    public bool IsInGameArea(Vector3 pos)
    {
        return pos.x >= MinX && pos.x <= MaxX && pos.y >= MinY && pos.y <= MaxY;
    }
}
