
using System;
using System.Collections.Generic;

[Serializable]
public class SaveScoreData
{
    public string name;
    public int score;
    public DateTime date;
    public PlayerType playerType;
}

[Serializable]
public class SaveScoreDataList
{
    public List<SaveScoreData> list = new List<SaveScoreData>();
}