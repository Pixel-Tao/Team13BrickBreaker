public enum PlayerType
{
    None,
    Player1,
    Player2
}

public struct PlayerData
{
    public PlayerType type;
    public int score;
    public int life;
    public bool isDead;
}
