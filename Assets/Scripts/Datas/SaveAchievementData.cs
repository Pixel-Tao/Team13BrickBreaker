
using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveAchievementData
{
    public int cleardStage;
    public string cleardDateTimeText;
}

[System.Serializable]
public class SaveAchievementDataList
{
    public List<SaveAchievementData> achievements = new();
}
