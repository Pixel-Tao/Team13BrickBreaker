using System;
using UnityEngine;

public class BounceBallStat : MonoBehaviour
{
    public PaddleStatData CurrentBallStat;
    [SerializeField] private float maxBallSpeed = 6f;
    [SerializeField] private int maxBallPower = 3;

    private BounceBall bounceBall;
    private PaddleStat paddleStat;

    private void Awake()
    {
        bounceBall = GetComponent<BounceBall>();
    }

    void Start()
    {
        paddleStat = bounceBall.Owner.GetComponent<PaddleStat>();
        CurrentBallStat.ballSpeed = paddleStat.BallDefaultStat.ballSpeed;
        CurrentBallStat.ballPower = paddleStat.BallDefaultStat.ballPower;
        CurrentBallStat.ballSpeedIncrement = paddleStat.BallDefaultStat.ballSpeedIncrement;
    }

    public void IncreaseSpeed()
    {
        if (maxBallSpeed < CurrentBallStat.ballSpeed)
            CurrentBallStat.ballSpeed += CurrentBallStat.ballSpeedIncrement;
    }

    public void ResetSpeed()
    {
        CurrentBallStat.ballSpeed = paddleStat.BallDefaultStat.ballSpeed;
    }

    public void IncreasePower(int itemPower)
    {
        if (CurrentBallStat.ballPower < maxBallPower)
        {
            CurrentBallStat.ballPower = Math.Min(CurrentBallStat.ballPower + itemPower, maxBallPower);
            float size = CurrentBallStat.ballPower * 0.1f + 1f;
            transform.localScale = new Vector3(size, size, 1);
        }
    }

    public void ResetPower()
    {
        CurrentBallStat.ballPower = paddleStat.BallDefaultStat.ballPower;
    }

    public void ApplyItemEffect(Item item)
    {
        Type type = item.GetType();
        if (type == typeof(BallPowerUpItem))
        {
            BallPowerUpItem powerItem = item as BallPowerUpItem;
            IncreasePower(powerItem.powerUpValue);
        }
        else if (type == typeof(BallSpeedDownItem))
        {
            ResetSpeed();
        }
        //else if (type == typeof(BallMultipleAddItem))
        //{
        //    BallMultipleAddItem ballMultipleAddItem = item as BallMultipleAddItem;
        //    GenerateBall(ballMultipleAddItem.ballAddCountValue);
        //}
    }
}
