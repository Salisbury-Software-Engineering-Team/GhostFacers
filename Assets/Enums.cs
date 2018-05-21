public enum TileTypes
{
    Empty = 0,
    Weapon = 200,
    Help = 100,
    Daemon = 400,
    Angel = 300,
    SpecialWeapon = 600,
    Monster = 500,
    Portal = 7,
}

public enum CardType
{
    None = 0,
    Help = 100,
    Weapon = 200,
    Angel = 300,
    Demon = 400,
    Monster = 500,
    Special = 600,
}

public enum PieceType
{
    None,
    Human,
    Ghost,
    Death,
    Angel,
    ArchAngel,
    Monster,
}

public enum StartingZone
{
    None = 0,
    Good = 1,
    Evil = 2,
}

public enum SideType
{
    None,
    Good,
    Evil,
}

/// <summary>
/// Different type of Phases a Turn can be in
/// </summary>
public enum Phase
{
    None,
    Movement,
    Draw,
    Attack,
    Defend,
    EndTurn,
}
