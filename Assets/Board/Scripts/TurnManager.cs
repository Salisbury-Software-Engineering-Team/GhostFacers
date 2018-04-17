using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{ 
    private CharacterPiece Piece;

    public void BeginTurn(int move)
    {
        StartCoroutine(TurnLoop(move));
    }

    IEnumerator TurnLoop(int move)
    {
        Piece = GameManager.instance.CurrentPiece;
        yield return StartCoroutine(MovementPhase(move));
        Debug.Log("Here");
        yield return DrawPhase();
        yield return AttackPhase();
        yield return EndTurnPhase();
    }

    IEnumerator MovementPhase(int move)
    {
        GameManager.instance.PhaseText.text = "Phase: Movement";
        Piece.DisplayAvaliableMovement(move); // display movement 
        yield return new WaitUntil(() => Piece.doneMove == true);
        Debug.Log("pIECE" +Piece.doneMove);
    }

    IEnumerator DrawPhase()
    {
        return null;
    }

    IEnumerator AttackPhase()
    {
        return null;
    }

    IEnumerator EndTurnPhase()
    {
        GameManager.instance.PhaseText.text = "Phase: End Turn";
        GameManager.instance.TurnStarted = false;
        Piece.EndTurn();
        return null;
    }

}
