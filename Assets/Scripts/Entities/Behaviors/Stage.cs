using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private bool isDebug;
    public StageData StageData { get; private set; }

    private void Awake()
    {
    }

    private void Start()
    {
        UIManager.Instance.ClosePopup<StageClearPopup>();
        UIManager.Instance.ShowPopup<StageStartPopup>().SetStage(StageData);
        if (isDebug)
            StageManager.Instance.SetBrickCount(1);
        else
            StageManager.Instance.SetBrickCount(transform.childCount);
    }

    public void SetData(StageData stageData)
    {
        StageData = stageData;
    }
}
