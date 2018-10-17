using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStat : ScriptableObject
{
    public String Name;

    //TODO: Finish adding stats
    public int Movement;
    [HideInInspector] public int Health;
    [HideInInspector] public int Attack;

    [SerializeField] private int m_Health;
    [SerializeField] private int m_Attack;
    [SerializeField] private int m_Weapons;
    [SerializeField] private int m_Help;

    public int StartHealth { get { return m_Health; } }
    public int StartAttack { get { return m_Attack; } }
    public int MaxWeapons {  get { return m_Weapons; } }
    public int MaxHelp {  get { return m_Help; } }
    public List<Card> WeaponHand;
    public List<Card> HelpHand;
    public List<Effect> ActiveEffects;

    public List<PieceType> AttackablePieces; // list of all attackable piece this piece can attack.
    public List<CardType> DrawTypes; // list of all drawable card types

    public SideType Side;
    public PieceType Type;

    public GameObject Model;
    public Color PieceColor;

    /// <summary>
    /// Reset stats for a new game.
    /// </summary>
    public void SetupStats()
    {
        Health = m_Health;
        Attack = m_Attack;
        WeaponHand.Clear();
        HelpHand.Clear();
        ActiveEffects.Clear();
    }

    /// <summary>
    /// check to see if a given card in in the characters hand.
    /// </summary>
    /// <returns></returns>
    public bool IsCardInHand(Card card)
    {
        bool ret = false;

        // does card belong to weapon hand
        ret = WeaponHand.Contains(card);
        if (ret)
        {
            WeaponHand.Remove(card);
            return true;
        }

        // does card belong to help hand
        ret = HelpHand.Contains(card);
        if (ret)
        {
            HelpHand.Remove(card);
            return true;
        }

        return false;
    }
}
