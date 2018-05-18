using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    private CharacterPiece _Piece;
    private TileTypes _CardType;

    private void Awake()
    {
        ResetVars();
    }

    /// <summary>
    /// Begin the drawing of a card phase.
    /// </summary>
    /// <param name="piece">Current piece that is looking to draw a card</param>
    public void DrawCard(CharacterPiece piece)
    {
        _Piece = piece;
        _CardType = _Piece.CurrentTile.Type.Type;
        DetermineDeck();
    }

    private void DetermineDeck()
    {
        switch (_CardType)
        {
            case TileTypes.Help:
                {
                    DrawHelp();
                    return;
                }
            case TileTypes.Monster:
                {
                    DrawMonster();
                    return;
                }
            case TileTypes.Weapon:
                {
                    DrawWeapon();
                    return;
                }
        }

    }

    /// <summary>
    /// Rest the varaibles to be ready for the next draw card.
    /// </summary>
    private void ResetVars()
    {
        _Piece = null;
        _CardType = TileTypes.Empty;
    }

    private void DrawHelp()
    {
      
    }

    private void DrawMonster()
    {
       
    }

    private void DrawWeapon()
    {

    }
}
