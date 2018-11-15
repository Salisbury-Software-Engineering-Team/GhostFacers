using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO: add a way to check to see if an effect will modify this.
/// </summary>

public class Roll : MonoBehaviour
{
    public int baseMovement;
    public int Movement; // amount of spaces the character can move.
    public bool rolled; // used to make sure users can not roll multipple times for sam character.
    private int MaxRoll = 12;
    public int modValue = 0;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Set all vars at start.
    /// </summary>
    private void Init()
    {
        baseMovement = 0;
        Movement = 0;
        rolled = false;
    }

    /// <summary>
    /// Roll the dice to the current characters movment. Only roll if there is a current character selected
    /// and dice has not currently been rolled.
    /// </summary>
    public void RollDice()
    {
        if (GameManager.instance.CurrentPiece != null && GameManager.instance.CurrentPiece.canMove)
        {
            ApplyEffects();
            Debug.Log("Done Activate");
            System.DateTime localDate = System.DateTime.Now;
            Debug.Log("Done Activate");

            System.Random rand = new System.Random(localDate.Millisecond);
            Debug.Log("Done Activate");

            baseMovement = rand.Next(MaxRoll-1) + 2;

            Debug.Log("Roll = " + baseMovement + "  ModValue = " + modValue);
            Movement = baseMovement + modValue;
            rolled = true;
            GameManager.instance.Turn.BeginTurn(Movement);
        }
    }

    public void DontRoll()
    {
        if (GameManager.instance.CurrentPiece != null && GameManager.instance.CurrentPiece.canMove)
            GameManager.instance.Turn.BeginTurn(-1);
    }

    private void ResetRoll()
    {
        baseMovement = 0;
        Movement = 0;
        modValue = 0;
    }

    public int ModifyRoll(int mod)
    {
        if (mod > MaxRoll)
            mod = MaxRoll;
        if (mod < -MaxRoll)
            mod = -MaxRoll;
        modValue = mod;
        return 0;
    }

    /// <summary>
    /// Applies any effects that are curently waiting to be used.
    /// </summary>
    private void ApplyEffects()
    {
        CharacterPiece piece = this.gameObject.GetComponent<CharacterPiece>();
        piece.ApplyEffectsStaggedForCurrentPhase();
    }



}
