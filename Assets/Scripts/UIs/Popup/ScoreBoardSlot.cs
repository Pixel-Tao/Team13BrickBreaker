using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardSlot : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;

    public void SetData(SaveScoreData data)
    {
        nameText.text = data.name;
        scoreText.text = data.score.ToString("#,##0");
        timeText.text = data.dateTimeText;
    }
}
