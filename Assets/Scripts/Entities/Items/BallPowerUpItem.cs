using UnityEngine;

public class BallPowerUpItem : Item
{
    public override ItemType ItemType { get; protected set; } = ItemType.BallPowerUp;

    [Range(1, 10)] public int powerUpValue = 1;
}
