using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private readonly int DEFAULT_LIFE = 3;

    private static GameManager _instance;
    public static GameManager Instance { get { Init(); return _instance; } }

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
    public event Action<Paddle> OnBallGenerateEvent;
    public event Action<PlayerType, int> OnLifeChangedEvent;
    public event Action<PlayerType, int> OnScoreChangedEvent;

    private Dictionary<PlayerType, PlayerData> players = new Dictionary<PlayerType, PlayerData>();

    /// <summary>
    /// 게임 데이터 초기화
    /// </summary>
    private void GameReset()
    {
        players.Clear();
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

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        // 테스트입니다.
        GameReset();
        //OnPlayerJoin?.Invoke(PlayerType.Player2);
        PlayerJoin(PlayerType.Player1);
    }

    public void BallGenerate(Paddle owner)
    {
        OnBallGenerateEvent?.Invoke(owner);
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

    public bool HasLife(PlayerType type)
    {
        PlayerData player = players[type];
        return player.life > 0 && player.isDead == false;
    }

    public void Dead(PlayerType type)
    {
        PlayerData playerData = players[type];
        playerData.isDead = true;
        players[type] = playerData;
    }

    public bool IsAlive(PlayerType type)
    {
        return players[type].isDead == false;
    }

    public void TryGameOver()
    {
        if(IsAlive(PlayerType.Player1) || IsAlive(PlayerType.Player2)) return;

        SceneManager.LoadScene("TitleScene");
    }

    public void PlayerJoin(PlayerType player)
    {
        PlayerData playerData = players[player];
        playerData.life = DEFAULT_LIFE;
        playerData.isDead = false;
        players[player] = playerData;

        // 플레이어 추가.
        OnPlayerJoinEvent?.Invoke(player);
    }
}
