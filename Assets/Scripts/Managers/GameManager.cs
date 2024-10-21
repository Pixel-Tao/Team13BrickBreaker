using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private readonly int DEFAULT_LIFE = 3;
    private readonly float DEFAULT_ITEM_DROP_RATE = 0.5f;

    private int dropableItemCount = 0;

    public event Action<PlayerType> OnPlayerJoinEvent;
    public event Action<Paddle, Vector3?> OnBallGenerateEvent;
    public event Action<PlayerType, int> OnLifeChangedEvent;
    public event Action<PlayerType, int> OnScoreChangedEvent;
    public event Action<Vector3, int> OnItemDropEvent;
    public event Action OnPlayerClearEvent;

    public Defines.PlayModeType PlayModeType { get; private set; } = Defines.PlayModeType.Single;

    private Dictionary<PlayerType, PlayerData> players;

    /// <summary>
    /// 게임 데이터 초기화
    /// </summary>
    public void PlayerReset()
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

    public void ClearEvent()
    {
        OnPlayerJoinEvent = null;
        OnBallGenerateEvent = null;
        OnItemDropEvent = null;
    }
    public void InitGame(int dropableItemCount)
    {
        this.dropableItemCount = dropableItemCount;
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        if (PlayModeType == Defines.PlayModeType.Single)
        {
            PlayerJoin(PlayerType.Player1);
        }
        else if (PlayModeType == Defines.PlayModeType.Multi)
        {
            PlayerJoin(PlayerType.Player1);
            PlayerJoin(PlayerType.Player2);
        }

        // 플레이어 정보 업데이트
        PlayerUpdate(PlayerType.Player1);
        PlayerUpdate(PlayerType.Player2);
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

        UIManager.Instance.ShowPopup<GameoverPopup>()?.Init();
    }

    public void Ending()
    {
        // TODO : 엔딩은 어떻게..?
        UIManager.Instance.ShowPopup<GameoverPopup>()?.Init();
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

    public void PlayerUpdate(PlayerType type)
    {
        OnScoreChangedEvent?.Invoke(type, players[type].score);
        OnLifeChangedEvent?.Invoke(type, players[type].life);
    }

    public void PlayerClear()
    {
        OnPlayerClearEvent?.Invoke();
    }

    public void DropItem(Vector3 pos)
    {
        if (UnityEngine.Random.Range(0f, 1f) <= DEFAULT_ITEM_DROP_RATE) return;
        OnItemDropEvent?.Invoke(pos, UnityEngine.Random.Range(0, dropableItemCount));
    }

    public void SetPlayMode(Defines.PlayModeType mode)
    {
        this.PlayModeType = mode;
    }

    #region Screen Area
    public float ScreenMinX { get; private set; }
    public float ScreenMaxX { get; private set; }
    public float ScreenMinY { get; private set; }
    public float ScreenMaxY { get; private set; }

    public void SetMinX(float x)
    {
        this.ScreenMinX = x;
    }

    public void SetMaxX(float x)
    {
        this.ScreenMaxX = x;
    }

    public void SetMinY(float y)
    {
        this.ScreenMinY = y;
    }

    public void SetMaxY(float y)
    {
        this.ScreenMaxY = y;
    }

    public bool IsInGameArea(Vector3 pos)
    {
        return pos.x >= ScreenMinX && pos.x <= ScreenMaxX && pos.y >= ScreenMinY && pos.y <= ScreenMaxY;
    }
    #endregion
}
