using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageLevelSO", menuName = "ScriptableObjects/StageLevel/StageLevelSO", order = 1)]
public class StageLevelSO : ScriptableObject
{
    public List<GameObject> stages;
}
