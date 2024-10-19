using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObjects/StageSO", order = 1)]
public class StageSO : ScriptableObject
{
    public List<StageData> stages;
}
