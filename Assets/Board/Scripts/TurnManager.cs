using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private CharacterPiece Piece;

    public void BeginTurn(int move)
    {
        Piece = GameManager.instance.CurrentPiece;
        Debug.Log("Begin Turn");
        MovementPhase(move);
        DrawPhase();
        AttackPhase();
        EndTurnPhase();
    }

    private void MovementPhase(int move)
    {
        Piece.DisplayAvaliableMovement(move); // display movement 
        while (!Piece.doneMove) { } // wait for piece to finish moving
    }

    private void DrawPhase()
    {

    }

    private void AttackPhase()
    {

    }

    private void EndTurnPhase()
    {
        GameManager.instance.TurnStarted = false;
        Piece.EndTurn();
    }

}
