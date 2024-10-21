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
        fadeUI = Instantiate(fadeUIPrefab).GetComponent<FadeUI>();
        titleUI = Instantiate(titleUIPrefab).GetComponent<TitleUI>();

        UIManager.Instance?.ClearEvent();
        StageManager.Instance?.ClearEvent();
        StageManager.Instance.InitStage(stageSO);

        UIManager.Instance.OnFadeEvent += Fade;

        UIManager.Instance.FadeIn();
        AudioManager.Instance.Title();
    }

    private void Fade(FadeType fadeType, Action fadedAction)
    {
        // fadeType에 따라 FadeIn 또는 FadeOut 실행
        fadeUI.Play(fadeType, fadedAction);
    }


}
