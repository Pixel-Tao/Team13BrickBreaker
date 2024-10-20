using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPopup : PopupBase
{
    [SerializeField] private GameObject achievementSlotPrefab;
    [SerializeField] private Transform content;

    private List<AchievementSlot> slots;

    private int currentAchievementIndex = 0;

    private void Start()
    {
        StageManager.Instance.OnCheckAchievementAndUnlockEvent += CheckAchievementAndUnlock;
    }

    private void CheckAchievementAndUnlock(int stage)
    {
        for (int i = currentAchievementIndex; i < slots.Count; i++)
        {
            if (slots[i].StageData.stage == stage)
            {
                currentAchievementIndex = i;
                slots[i].UnlockAchievement();
                return;
            }
        }
    }

    private void InitSlots()
    {
        List<StageData> stages = StageManager.Instance.StageSO.stages;
        if (slots == null)
        {
            slots = new List<AchievementSlot>();
            for (int i = 0; i < stages.Count; i++)
            {
                GameObject slotObj = Instantiate(achievementSlotPrefab, content);
                AchievementSlot slot = slotObj.GetComponent<AchievementSlot>();
                slot.SetData(stages[i]);
                slot.LockAchievement();
                slots.Add(slot);
            }
        }
    }

    public override void LoadData()
    {
        InitSlots();

        SaveAchievementDataList dataList = SaveManager.Instance.Load<SaveAchievementDataList>();
        if (dataList == null) dataList = new SaveAchievementDataList();

        for (int i = 0; i < dataList.achievements.Count; i++)
        {
            CheckAchievementAndUnlock(dataList.achievements[i].cleardStage);
        }
    }
}
