using System;
using UnityEngine;
using UnityEngine.UI;

public class StageClearPopup : PopupBase
{
    [SerializeField] private Text stageText;
    
    public void SetStage(StageData stageData)
    {
        stageText.text = $"Stage {stageData.stage} Clear!";
        AutoClose();
    }
    private void AutoClose()
    {
        Invoke("Close", 1f);
    }

    private void Close()
    {
        UIManager.Instance.ClosePopup(this);
        StageManager.Instance.LoadStage();
    }
}
