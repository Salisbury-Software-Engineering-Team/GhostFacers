using UnityEngine;

public enum TileTypes
{
    Empty,
    Weapon,
    Help,
    Daemon,
    Angel,
    SpecialWeapon,
    Green,
    Portal,
}

[CreateAssetMenu]
public class TileType : ScriptableObject
{
	public string TileName;
    public Sprite TileImage;
    public Color ButtonColor;
    public TileTypes Type;
}