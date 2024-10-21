using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardPopup : PopupBase
{
    [SerializeField] private GameObject scoreBoardSlotPrefab;
    [SerializeField] private Transform content;

    private ScoreBoardSlot[] scoreBoardSlots = new ScoreBoardSlot[10];

    public override void LoadData()
    {
        SaveScoreDataList dataList = SaveManager.Instance.Load<SaveScoreDataList>();

        for (int i = 0; i < scoreBoardSlots.Length; i++)
        {
            if (scoreBoardSlots[i] == null)
            {
                scoreBoardSlots[i] = Instantiate(scoreBoardSlotPrefab, content).GetComponent<ScoreBoardSlot>();
            }

            if (i >= dataList.scores.Count)
            {
                scoreBoardSlots[i].gameObject.SetActive(false);
                continue;
            }

            scoreBoardSlots[i].SetData(dataList.scores[i]);
            scoreBoardSlots[i].gameObject.SetActive(true);
        }
    }
}
