using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private int brickCount;
    public StageData StageData { get; private set; }

    private void Awake()
    {
        GameManager.Instance.OnIncreaseBrickEvent += IncreaseBrickCount;
        GameManager.Instance.OnDecreaseBrickEvent += DecreaseBrickCount;
    }

    private void Start()
    {
        UIManager.Instance.ClosePopup<StageClearPopup>();
        UIManager.Instance.ShowPopup<StageStartPopup>().SetStage(StageData);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnIncreaseBrickEvent -= IncreaseBrickCount;
        GameManager.Instance.OnDecreaseBrickEvent -= DecreaseBrickCount;
    }

    public void SetData(StageData stageData)
    {
        StageData = stageData;
    }

    private void DecreaseBrickCount()
    {
        brickCount--;
        if (brickCount <= 0)
        {
            StageClear();
        }
    }

    private void StageClear()
    {
        UIManager.Instance.ShowPopup<StageClearPopup>().SetStage(StageData);
        GameManager.Instance.PlayerClear();
        Invoke("ApplyStageClear", 2f);
    }

    private void ApplyStageClear()
    {
        GameManager.Instance.StageClear();
        Destroy(gameObject);
    }

    private void IncreaseBrickCount()
    {
        brickCount++;
    }
}
