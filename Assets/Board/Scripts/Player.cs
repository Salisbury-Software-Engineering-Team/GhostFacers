using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SideType
{
    None,
    Good,
    Evil,
}

public class Player : MonoBehaviour
{
    public List<CharacterPiece> Pieces;
    private List<CharacterPiece> DeadPieces;

    public int TotalPieceCount;
    public int TotalPiecesLeftToMove;

    public SideType Side;

    /* TODO:::
     * Add an event to handle when a piece dies. Will be called from charcter script.
     */

    private void Start()
    {
        TotalPieceCount = Pieces.Count;
        DeadPieces = new List<CharacterPiece>();
    }

    private void OnValidate()
    {
        //TotalPieceCount = Pieces.Count;
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

    private void Update()
    {
        // ************* TESTING ************************************
        // Check to see if any piece died and remove it from active pieces. Also
        // adds the piece to deadPiece list for future use if needed.
        CharacterPiece p = null;
        foreach (CharacterPiece piece in Pieces)
        {
            if (piece.Died)
            {
                p = piece;
                Debug.Log("Play found Piece Died " + piece);
                DeadPieces.Add(piece);
                TotalPieceCount--;
                break;
            }
        }
        if (p)
            Pieces.Remove(p);
    }

}
