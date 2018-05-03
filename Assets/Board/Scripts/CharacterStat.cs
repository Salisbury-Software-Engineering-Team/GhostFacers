using System.Collections.Generic;
using System;
using UnityEngine;

public enum PieceType
{
    None,
    Human,
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
    [HideInInspector] public int Health;
    [SerializeField] private int m_Health;
    public int Movement;
    [HideInInspector] public int Attack;
    [SerializeField] private int m_Attack;

    [SerializeField] private int m_Weapons;
    [SerializeField] private int m_MaxHelp;

    [SerializeField] private List<Card> Weapons;
    [SerializeField] private List<Card> Help;
    [SerializeField] private List<Effect> ActiveEffects;

    public SideType Side;
    public PieceType Type;

    public GameObject Model;
    public Color PieceColor;

    public void SetupStats()
    {
        Health = m_Health;
        Attack = m_Attack;
    }
}
