using System;
using System.Collections.Generic;
using UnityEngine;

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

    public Action<PlayerType> OnPlayerJoin;
    public Action<PlayerType, int> OnLifeChanged;
    public Action<PlayerType, int> OnScoreChanged;

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
            playerData.life = DEFAULT_LIFE;
            playerData.score = 0;
            playerData.isDead = false;
            players.Add(playerType, playerData);
            OnScoreChanged?.Invoke(playerData.type, playerData.score);
            OnLifeChanged?.Invoke(playerData.type, playerData.life);
        }
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        // 테스트입니다.
        GameReset();
        // 플레이어 추가.
        OnPlayerJoin?.Invoke(PlayerType.Player1);
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
        OnScoreChanged?.Invoke(type, playerData.score);
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
        OnLifeChanged?.Invoke(type, playerData.life);
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
        OnLifeChanged?.Invoke(type, playerData.life);
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

    public void TitleStart()
    {
        // dev에서 수정함.
    }
}
