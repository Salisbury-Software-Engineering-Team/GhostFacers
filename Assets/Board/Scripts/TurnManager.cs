using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Different type of Phases a Turn can be in
/// </summary>
public enum Phase
{
    None,
    Movement,
    Draw,
    Attack,
    Defend,
    EndTurn,
}

public class TurnManager : MonoBehaviour
{
    //Character piece
    [SerializeField] private CharacterPiece _piece;
    public CharacterPiece Piece
    {   
        get { return _piece; }
    }

    //What phase the turn is curently in
    [SerializeField] private Phase _turnPhase;
    public Phase TurnPhase
    {
        get { return _turnPhase; }
    }

    public GameObject BtnDontMove;
    private Attack _Attack;

    private void Awake()
    {
        _Attack = GetComponent<Attack>();
        BtnDontMove.SetActive(false);
    }

    public void BeginTurn(int move)
    {
        GameManager.instance.TurnStarted = true;
        StartCoroutine(TurnLoop(move));
    }

    IEnumerator TurnLoop(int move)
    {
        _piece = GameManager.instance.CurrentPiece;
        yield return MovementPhase(move);
        yield return DrawPhase();
        Debug.Log("Begin Attacking");
        yield return AttackPhase();
        Debug.Log("Done Attacking");
        Debug.Log("Begin EndTurn");
        yield return EndTurnPhase();
        Debug.Log("Done Turn");
        _turnPhase = Phase.None;
    }

    IEnumerator MovementPhase(int move)
    {
        _turnPhase = Phase.Movement;
        GameManager.instance.PhaseText.text = "Phase: Movement";
        BtnDontMove.SetActive(true);
        _piece.DisplayAvaliableMovement(move); // display movement 
        yield return new WaitUntil(() => _piece.doneMove == true);
    }

    IEnumerator DrawPhase()
    {
        _turnPhase = Phase.Draw;
        return null;
    }

    IEnumerator AttackPhase()
    {
        _turnPhase = Phase.Attack;
        GameManager.instance.PhaseText.text = "Phase: Attack";
        _Attack.BeginAttack(Piece); // start the attack
        yield return new WaitUntil(() => _Attack.doneAttack == true); // wait till attack sone
    }

    IEnumerator EndTurnPhase()
    {
        _turnPhase = Phase.EndTurn;
        GameManager.instance.PhaseText.text = "Phase: End Turn";
        GameManager.instance.TurnStarted = false;
        _piece.EndTurn();
        GameManager.instance.CurrentPlayer.TotalPiecesLeftToMove--;
        return null;
    }

    public void DontMove()
    {
        BtnDontMove.SetActive(false);
        _piece.ClearHighlights();
        _piece.doneMove = true;
    }

}
