using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { Init(); return _instance; } }

    public event Action<FadeType, Action> OnFadeEvent;

    private static void Init()
    {
        if (_instance == null)
        {
            // GameManager 동적 생성
            GameObject go = new GameObject { name = "UIManager" };
            _instance = go.AddComponent<UIManager>();
            DontDestroyOnLoad(go);
        }
    }

    public void SetEvent(Action<FadeType, Action> method)
    {
        OnFadeEvent = null;
        OnFadeEvent += method;
    }

    public void FadeIn(Action fadedAction = null)
    {
        // 검은 화면이 점점 밝아지는 효과
        OnFadeEvent?.Invoke(FadeType.FadeIn, fadedAction);
    }

    public void FadeOut(Action fadedAction = null)
    {
        // 밝은 화면 점점 어두어지는 효과
        OnFadeEvent?.Invoke(FadeType.FadeOut, fadedAction);
    }
}
