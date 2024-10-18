
using UnityEngine;

[System.Serializable]
public struct PaddleStatData
{
    [Range(1f, 10f)] public float ballSpeed;
    [Range(0, 1f)] public float ballSpeedIncrement;
    [Range(1, 20)] public int ballPower;
}
