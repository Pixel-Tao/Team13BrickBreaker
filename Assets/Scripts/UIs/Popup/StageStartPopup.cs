using UnityEngine;
using UnityEngine.UI;

public class StageStartPopup : PopupBase
{
    [SerializeField] private Text stageText;
    [SerializeField] private Text titleText;

    public void SetStage(StageData stageData)
    {
        stageText.text = $"Stage {stageData.stage}";
        titleText.text = stageData.title;
        AutoClose();
    }

    private void AutoClose()
    {
        Invoke("Close", 2f);
    }

    private void Close()
    {
        UIManager.Instance.ClosePopup(this);
        GameManager.Instance.GameStart();
    }
}
