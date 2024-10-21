using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObjects/StageSO", order = 1)]
public class StageSO : ScriptableObject
{
    [Header("첫번째 스테이지 (Stages.Count 이상은 바로 게임 오버입니다.)")]
    [Range(1, 99)] public int firstStage;

    [Header("스테이지 목록")]
    public List<StageData> stages;
}
