using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SideType
{
    None,
    Good,
    Evil,
}

[Serializable]
public class Player
{
    public List<CharacterPiece> Pieces;
    private List<CharacterPiece> DeadPieces;

    public int TotalPieceCount;
    public int TotalPiecesLeftToMove;

    public SideType Side;

    /* TODO:::
     * Add an event to handle when a piece dies. Will be called from charcter script.
     */

    public Player(List<CharacterPiece> p, SideType s)
    {
        Pieces = p;
        Side = s;
        TotalPieceCount = Pieces.Count;
        DeadPieces = new List<CharacterPiece>();
        foreach (CharacterPiece piece in Pieces)
        {
            piece.DeathHandler += (pi) => PieceDied(pi);
        }
    }

    public Player(SideType s) : this(new List<CharacterPiece>(), s) { }

    public void SetUpTurn()
    {
        TotalPiecesLeftToMove = TotalPieceCount;
        foreach (CharacterPiece piece in Pieces)
        {
            if (piece)
            {
                piece.SetupTurn();
            }
        }
    }

    private void PieceDied(CharacterPiece piece)
    {
        CharacterPiece p = null;
        p = piece;
        Debug.Log("Play found Piece Died " + piece);
        DeadPieces.Add(piece);
        TotalPieceCount--;
        Pieces.Remove(p);
    }

}
