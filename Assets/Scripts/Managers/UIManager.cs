using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public event Action<FadeType, Action> OnFadeEvent;

    private Dictionary<string, PopupBase> popups = new Dictionary<string, PopupBase>();

    public bool IsPause { get; private set; }

    public void ClearEvent()
    {
        OnFadeEvent = null;
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

    public T ShowPopup<T>() where T : PopupBase
    {
        if (popups.TryGetValue(typeof(T).Name, out PopupBase popup))
        {
            popup.gameObject.SetActive(true);
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/UIs/{typeof(T).Name}");
            if (prefab == null) return null;
            popup = Instantiate(prefab, transform).GetComponent<PopupBase>();
            popups.Add(typeof(T).Name, popup);
        }

        return popup as T;
    }

    public void ClosePopup<T>() where T : PopupBase
    {
        Type type = typeof(T);

        if (popups.TryGetValue(type.Name, out PopupBase popupBase))
        {
            popupBase.gameObject.SetActive(false);
        }
    }

    public void ClosePopup<T>(T popup) where T : PopupBase
    {
        if (popups.TryGetValue(popup.GetType().Name, out PopupBase popupBase))
        {
            popupBase.gameObject.SetActive(false);
        }
    }

    public void Pause()
    {
        if (IsPause) return;

        IsPause = true;
        ShowPopup<PausePopup>()?.Init();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (IsPause == false) return;

        IsPause = false;
        Time.timeScale = 1f;
        ClosePopup<PausePopup>();
    }
}
