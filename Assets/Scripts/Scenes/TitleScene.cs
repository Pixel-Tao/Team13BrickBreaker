using System;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject titleUIPrefab;
    [SerializeField] private GameObject fadeUIPrefab;
    [SerializeField] private StageSO stageSO;

    private TitleUI titleUI;
    private FadeUI fadeUI;

    private void Start()
    {
        // TitleScene 진입점.
        // UI 초기화
        InitUI();
        // Event 초기화
        InitEvent();
        // 스테이지 초기화
        StageManager.Instance.InitStage(stageSO);
        // 화면을 점차적으로 밝게 한다.
        UIManager.Instance.FadeIn();
        // GameScene에서 사운드 재생
        AudioManager.Instance.TitlePlay();
    }

    private void InitUI()
    {
        fadeUI = Instantiate(fadeUIPrefab).GetComponent<FadeUI>();
        titleUI = Instantiate(titleUIPrefab).GetComponent<TitleUI>();
    }

    private void InitEvent()
    {
        UIManager.Instance?.ClearEvent();
        StageManager.Instance?.ClearEvent();

        UIManager.Instance.OnFadeEvent += Fade;
    }

    private void Fade(FadeType fadeType, Action fadedAction)
    {
        // fadeType에 따라 FadeIn 또는 FadeOut 실행
        fadeUI.Play(fadeType, fadedAction);
    }


}
