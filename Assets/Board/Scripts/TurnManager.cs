using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Different type of Phases a Turn can be in
/// </summary>
public enum Phase
{
    None,
    Movement,
    Draw,
    Attack,
    EndTurn,
}

public class TurnManager : MonoBehaviour
{ 
    private CharacterPiece Piece;
    private Phase _turnPhase;
    public Phase TurnPhase
    {
        get { return _turnPhase; }
    }

    public void BeginTurn(int move)
    {
        GameManager.instance.TurnStarted = true;
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
        _turnPhase = Phase.None;
    }

    IEnumerator MovementPhase(int move)
    {
        _turnPhase = Phase.Movement;
        GameManager.instance.PhaseText.text = "Phase: Movement";
        Piece.DisplayAvaliableMovement(move); // display movement 
        yield return new WaitUntil(() => Piece.doneMove == true);
        Debug.Log("pIECE" +Piece.doneMove);
    }

    IEnumerator DrawPhase()
    {
        _turnPhase = Phase.Draw;
        return null;
    }

    IEnumerator AttackPhase()
    {
        _turnPhase = Phase.Attack;
        Debug.Log("Begin Attacking");
        GameManager.instance.CurrentPiece.AttackScript.BeginAttack();
        yield return new WaitUntil(() => Piece.AttackScript.doneAttack == true);
    }

    IEnumerator EndTurnPhase()
    {
        _turnPhase = Phase.EndTurn;
        GameManager.instance.PhaseText.text = "Phase: End Turn";
        GameManager.instance.TurnStarted = false;
        Piece.EndTurn();
        return null;
    }

}
