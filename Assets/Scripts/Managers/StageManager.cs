using System;

public class StageManager : Singleton<StageManager>
{
    public event Action<int> OnCheckAchievementAndUnlockEvent;
    public event Action OnStageClearEvent;

    public int SelectedStage { get; private set; }
    public Stage CurrentStage { get; private set; }
    public StageSO StageSO { get; private set; }
    public int BrickCount { get; private set; }

    private bool isStageClear = false;

    public void InitStage(StageSO stageSO)
    {
        this.StageSO = stageSO;
        this.BrickCount = 0;
        this.SelectedStage = StageSO.firstStage;
    }

    public void LoadStage()
    {
        if (0 >= SelectedStage || SelectedStage > StageSO.stages.Count)
        {
            GameManager.Instance.Ending();
            return;
        }

        isStageClear = false;
        StageData stageData = StageSO.stages[SelectedStage - 1];
        CurrentStage = Instantiate(stageData.stagePrefab).GetComponent<Stage>();
        CurrentStage?.SetData(stageData);
    }

    public void ClearEvent()
    {
        OnCheckAchievementAndUnlockEvent = null;
    }

    public void UnlockAchievement(int stage)
    {
        SaveAchievementDataList dataList = SaveManager.Instance.Load<SaveAchievementDataList>();
        if (dataList == null) dataList = new SaveAchievementDataList();

        for (int i = 0; i < dataList.achievements.Count; i++)
        {
            if (dataList.achievements[i].cleardStage == stage)
                return;
        }

        dataList.achievements.Add(new SaveAchievementData { cleardStage = stage, cleardDateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        SaveManager.Instance.Save(dataList);
        OnCheckAchievementAndUnlockEvent?.Invoke(stage);
    }

    public void DecreaseBrickCount()
    {
        BrickCount--;
    }

    public void SetBrickCount(int count)
    {
        BrickCount = count;
    }

    public void TryStageClear()
    {
        if (BrickCount > 0) return;
        if (isStageClear) return;
        isStageClear = true;
        UIManager.Instance.ShowPopup<StageClearPopup>().SetStage(CurrentStage.StageData);
        // 업적 업데이트 하고
        UnlockAchievement(SelectedStage);
        // 플레이어 정리 하고
        GameManager.Instance.PlayerClear();
        // 스테이지 오브젝트 파괴
        Destroy(CurrentStage.gameObject);
        // 다음 스테이지 로드
        SelectedStage++;
    }
}
