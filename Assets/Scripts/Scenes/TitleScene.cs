using System;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject titleUIPrefab;
    [SerializeField] private GameObject fadeUIPrefab;

    private TitleUI titleUI;
    private FadeUI fadeUI;

    private void Start()
    {
        // TitleScene 진입점.
        fadeUI = Instantiate(fadeUIPrefab).GetComponent<FadeUI>();
        titleUI = Instantiate(titleUIPrefab).GetComponent<TitleUI>();

        UIManager.Instance.OnFadeEvent += Fade;

        UIManager.Instance.FadeIn();
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnFadeEvent -= Fade;
    }

    private void Fade(FadeType fadeType, Action fadedAction)
    {
        // fadeType에 따라 FadeIn 또는 FadeOut 실행
        fadeUI.Play(fadeType, fadedAction);
    }
}
