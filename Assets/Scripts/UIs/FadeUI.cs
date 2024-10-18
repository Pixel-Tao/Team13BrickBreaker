using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public enum FadeType
{
    FadeIn,
    FadeOut
}

public class FadeUI : MonoBehaviour
{
    private Action fadedAction;

    private FadeType fadeType;
    [SerializeField] private Image screen;

    [SerializeField][Header("효과 전환 시간")] private float duration = 1f;
    private float fadeTime = 0f;
    private bool isPlaying = false;

    public void Start()
    {
    }

    private void Update()
    {
        FadeUpdate();
    }

    private void FadeUpdate()
    {
        if (isPlaying)
        {
            fadeTime += Time.deltaTime;

            if (fadeType == FadeType.FadeIn)
            {
                FadeIn();
            }
            else if (fadeType == FadeType.FadeOut)
            {
                FadeOut();
            }
        }
    }

    public void Play(FadeType fadeType, Action fadedAction = null)
    {
        this.isPlaying = true;
        this.fadeType = fadeType;
        this.fadedAction = fadedAction;

        if (fadeType == FadeType.FadeIn)
            screen.color = new Color(0, 0, 0, 1);
        else
            screen.color = new Color(0, 0, 0, 0);

        fadeTime = 0;
        gameObject.SetActive(true);
    }

    private void FadeIn()
    {
        float alpha = Mathf.Max(1 - (fadeTime / (duration)), 0);
        AlphaChange(alpha);

        if (fadeTime >= duration)
        {
            isPlaying = false; 
            CallFadedAction();
            gameObject.SetActive(false);
        }
    }

    private void FadeOut()
    {
        float alpha = Mathf.Min(fadeTime / duration, 1);
        AlphaChange(alpha);

        if (fadeTime >= duration)
        {
            isPlaying = false;
            CallFadedAction();
        }
    }

    private void AlphaChange(float alpha)
    {
        Color color = screen.color;
        color.a = alpha;
        screen.color = color;
    }

    private void CallFadedAction()
    {
        fadeTime = 0;
        fadedAction?.Invoke();
    }
}
