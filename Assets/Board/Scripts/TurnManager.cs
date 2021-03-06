﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    //Character piece
    [SerializeField] private CharacterPiece _piece;
    public CharacterPiece Piece
    {   
        get { return _piece; }
    }

    //What the turn is curently in
    [SerializeField] private Phase _turnPhase;
    public Phase TurnPhase
    {
        get { return _turnPhase; }
    }

    [SerializeField] private GameObject _BtnDontMove;
    [SerializeField] private Text _HelpText;
    private Attack _Attack;
    private Deck _Deck;

    private void Awake()
    {
        _Attack = GetComponent<Attack>();
        _Deck = GetComponent<Deck>();
        _BtnDontMove.SetActive(false);
        _HelpText.text = "Select Piece to Move";
    }

    public void BeginTurn(int move)
    {
        GameManager.instance.TurnStarted = true;
        StartCoroutine(TurnLoop(move));
        DisplayRollAmount();
    }

    IEnumerator TurnLoop(int move)
    {
        _piece = GameManager.instance.CurrentPiece;
        Debug.Log("Begin Movement");
        yield return MovementPhase(move);
        yield return DrawPhase();
        Debug.Log("Begin Attacking");
        yield return AttackPhase();
        Debug.Log("Done Attacking");
        Debug.Log("Begin EndTurn");
        yield return EndTurnPhase();
        Debug.Log("Done Turn");
        _turnPhase = Phase.Roll;
    }

    IEnumerator MovementPhase(int move)
    {
        // Movement turn skiped
        if (move == -1)
        {
            _piece.canMove = false;
            _piece.doneMove = true;
        }
        else
        {
            DisplayRollAmount();
            _piece.canMove = false;
            _turnPhase = Phase.Movement;
            _BtnDontMove.SetActive(true);
            _piece.DisplayAvaliableMovement(move); // display movement 
            yield return new WaitUntil(() => _piece.doneMove == true);
        }
    }

    IEnumerator DrawPhase()
    {
        _HelpText.text = "Draw a Card";
        _turnPhase = Phase.Draw;
        _Deck.DrawCard(_piece);
        yield return new WaitUntil(() => _Deck.DoneDraw == true);
    }

    IEnumerator AttackPhase()
    {
        _HelpText.text = "Select Piece to Attack";
        _turnPhase = Phase.Attack;
        _Attack.SetupAttack(Piece); // start the attack
        yield return new WaitUntil(() => _Attack.DoneAttack == true); // wait till attack done
    }

    IEnumerator EndTurnPhase()
    {
        GameManager.instance.CurrentPiece = null;
        _HelpText.text = "Select Piece to Move";
        _turnPhase = Phase.EndTurn;
        GameManager.instance.TurnStarted = false;
        _piece.EndTurn();
        GameManager.instance.CurrentPlayer.TotalPiecesLeftToMove--;
        _piece = null;
        return null;
    }

    public void DontMove()
    {
        _BtnDontMove.SetActive(false);
        _piece.ClearHighlights();
        _piece.doneMove = true;
    }

    public void Moving()
    {
        _BtnDontMove.SetActive(false);
    }

    private void ClearHelpText()
    {
        _HelpText.text = "";
    }

    /// <summary>
    /// Display the current Rolled amount.
    /// </summary>
    private void DisplayRollAmount()
    {
        if (_HelpText)
        {
            String txt = "Total Rolled: ";
            txt += + _piece.GetComponent<Roll>().Movement;
            if (_piece.GetComponent<Roll>().modValue > 0)
            {
                txt += " ("+ _piece.GetComponent<Roll>().baseMovement+ " + " + _piece.GetComponent<Roll>().modValue + ")";
            }
            _HelpText.text = txt;
        }
        else
            Debug.Log("Error on DiplayRollAmount(): NO gameObject for Txt display.");
    }

}
