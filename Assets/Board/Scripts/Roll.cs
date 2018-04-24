using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public int Movement; // amount of spaces the character can move.
    public bool rolled; // used to make sure users can not roll multipple times for sam character.
    private int MaxRoll = 12;

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
            rolled = true;
            GameManager.instance.Turn.BeginTurn(Movement);
        }
    }
	
}
