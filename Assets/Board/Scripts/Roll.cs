using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: add a way to check to see if an effect will modify this.
/// </summary>

public class Roll : MonoBehaviour
{
    public int Movement; // amount of spaces the character can move.
    public bool rolled; // used to make sure users can not roll multipple times for sam character.
    private int MaxRoll = 12;
    private int modValue = 0;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Set all vars at start.
    /// </summary>
    private void Init()
    {
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

            System.DateTime localDate = System.DateTime.Now;
            System.Random rand = new System.Random(localDate.Millisecond);
            Movement = rand.Next(MaxRoll-1) + 2;

            Debug.Log("Roll = " + Movement + "  ModValue = " + modValue);
            Movement = Movement + modValue;
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

}
