using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public event Action<FadeType, Action> OnFadeEvent;

    // 열린 팝업을 관리하는 PopupBase Dictionary
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
            // Dictionary에 팝업이 있는 경우 활성화한다.
            popup.gameObject.SetActive(true);
        }
        else
        {
            // Dictionary에 팝업이 없는 경우 Prefab을 로드하여 생성한다.
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/UIs/{typeof(T).Name}");
            if (prefab == null) return null;
            popup = Instantiate(prefab, transform).GetComponent<PopupBase>();
            // Popup을 생성했다면 Dictionary에 추가한다.
            popups.Add(typeof(T).Name, popup);
        }

        return popup as T;
    }

    // PopupBase를 상속받은 T 클래스의 팝업을 닫아준다.
    public void ClosePopup<T>() where T : PopupBase
    {
        Type type = typeof(T);

        if (popups.TryGetValue(type.Name, out PopupBase popupBase))
        {
            // 실질적으로 Popup을 닫는 것이 아닌 일시적으로 숨김처리를 한다.
            // (Pooling)
            popupBase.gameObject.SetActive(false);
        }
    }

    public void ClosePopup<T>(T popup) where T : PopupBase
    {
        if (popups.TryGetValue(popup.GetType().Name, out PopupBase popupBase))
        {
            // 실질적으로 Popup을 닫는 것이 아닌 일시적으로 숨김처리를 한다.
            // (Pooling)
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
