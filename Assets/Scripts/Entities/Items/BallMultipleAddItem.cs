using UnityEngine;

public class BallMultipleAddItem : Item
{
    public override ItemType ItemType { get; protected set; } = ItemType.BallMultipleAdd;

    [Range(1, 10)] public int ballAddCountValue = 1;

}
