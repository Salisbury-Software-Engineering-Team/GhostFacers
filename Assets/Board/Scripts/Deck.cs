using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private CharacterPiece _Piece;
    private TileTypes _CardType;

    private Queue<Card> _DeckMonster;
    private Queue<Card> _DeckHelp;
    private Queue<Card> _DeckWeapon;

    private void Awake()
    {
        ResetVars();
        CreateDecks();
    }

    private void CreateDecks()
    {
        _DeckWeapon = new Queue<Card>();
        _DeckHelp = new Queue<Card>();
        _DeckMonster = new Queue<Card>();

        // Create wepon deck
        Card[] weapons = Resources.LoadAll<Card>("Weapon/Playable");
        Card[] help = Resources.LoadAll<Card>("Help/Playable");
        Card[] monster = Resources.LoadAll<Card>("Monster/Playable");

        foreach (Card card in weapons)
        {
            _DeckWeapon.Enqueue(card);
        }
        Debug.Log("Weapon Deck Created. Size = " + _DeckWeapon.Count);

        foreach (Card card in help)
        {
            _DeckHelp.Enqueue(card);
        }
        Debug.Log("Help Deck Created. Size = " + _DeckHelp.Count);

        foreach (Card card in monster)
        {
            _DeckMonster.Enqueue(card);
        }

        Debug.Log("Monster Deck Created. Size = " + _DeckMonster.Count);
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

    private bool CanPieceDraw()
    {
        int cardType = (int)_Piece.CurrentTile.Type.Type;
        CardType type = (CardType)cardType;

        if (type == CardType.Special) // all piece can draw special
            return true;
        else 
            return _Piece.Stat.DrawTypes.Contains(type);        
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
        //Draw the top card
        Card Top = _DeckWeapon.Dequeue();

        //Add to the current pieces hand.
        Card removedCard = _Piece.AddCard(Top);
        if (removedCard)
            _DeckWeapon.Enqueue(removedCard);
    }

    private void RandomizeDecks()
    {

    }
}
