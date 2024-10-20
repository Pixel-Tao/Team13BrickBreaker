
using System;
using System.Collections.Generic;

[Serializable]
public class SaveScoreData
{
    public string name;
    public int score;
    public string dateTimeText;
    public PlayerType playerType;
}

[Serializable]
public class SaveScoreDataList
{
    public List<SaveScoreData> scores = new List<SaveScoreData>();
}