using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public void GameStart()
    {

    }

    public void TitleStart()
    {

    }
}
