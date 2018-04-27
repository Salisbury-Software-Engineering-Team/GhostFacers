using System.Collections.Generic;
using System;
using UnityEngine;

public enum PieceType
{
    Ghost,
    Death,
    Angel,
    ArchAngel,
}

[CreateAssetMenu]
public class CharacterStat : ScriptableObject
{
    public String Name;

    //TODO: Finish adding stats
    [SerializeField] private int HealthLeft;
    [SerializeField] private int HealthTotal;
    [SerializeField] private int Movement;

    [SerializeField] private int MaxWeapons;
    [SerializeField] private int MaxHelp;

    [SerializeField] private List<Card> Weapons;
    [SerializeField] private List<Card> Help;
    [SerializeField] private List<Effect> ActiveEffects;

    public SideType Side;
    public PieceType Type;

    public GameObject Model;
    public Color PieceColor;
}
