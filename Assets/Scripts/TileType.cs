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
}

[CreateAssetMenu]
public class TileType : ScriptableObject
{
	public string TileName;
    public Sprite TileImage;
    public Color ButtonColor;

    public TileTypes Type;

}