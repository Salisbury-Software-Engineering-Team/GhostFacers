using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SideType
{
    Good,
    Evil,
}

public class Player : MonoBehaviour
{
    public List<CharacterPiece> Pieces;

    public int TotalPieceCount;
    public int TotalPiecesLeftToMove;

    public SideType Side;

    private void OnValidate()
    {
        TotalPieceCount = Pieces.Count;
    }

    public void SetUpTurn()
    {
        TotalPiecesLeftToMove = TotalPieceCount;
        foreach (CharacterPiece piece in Pieces)
        {
            if (piece)
                piece.SetupTurn();
        }
    }

}
