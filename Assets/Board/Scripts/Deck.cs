using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    private CharacterPiece _Piece;
    private TileTypes _CardType;
    private int answer = -1;
    private bool _doneDraw;

    private Queue<Card> _DeckMonster;
    private Queue<Card> _DeckHelp;
    private Queue<Card> _DeckWeapon;

    public Button BtnNo;
    public Button BtnYes;
    public GameObject DrawPanel;
    public Text DrawTitle;
    public bool DoneDraw { get { return _doneDraw; } }

    private void Awake()
    {
        ResetVars();
        CreateDecks();
        BtnYes.onClick.AddListener(YesClicked);
        BtnNo.onClick.AddListener(NoClicked);
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

        RandomlyEnqueueCards(weapons, _DeckWeapon);
        RandomlyEnqueueCards(help, _DeckHelp);
        RandomlyEnqueueCards(monster, _DeckMonster);
    }

    /// <summary>
    /// Begin the drawing of a card phase.
    /// </summary>
    /// <param name="piece">Current piece that is looking to draw a card</param>
    public void DrawCard(CharacterPiece piece)
    {
        StartCoroutine(DrawLoop(piece));
    }

    private IEnumerator DrawLoop(CharacterPiece piece)
    {
        ResetVars();
        _Piece = piece;
        _CardType = _Piece.CurrentTile.Type.Type;
        int temp = (int)_Piece.CurrentTile.Type.Type;
        Debug.Log(temp);
        if (CanPieceDraw())
        {
            Debug.Log(_Piece + " can Draw.");
            DrawPanel.SetActive(true);
            string cardTypeName = Enum.GetName(typeof(TileTypes), _CardType);
            DrawTitle.text = "Would you like to Draw a " + cardTypeName + " Card?";
            yield return WaitForAnswer();
            DrawPanel.SetActive(false);
            //user said yes
            if (answer == 1)
                DetermineDeck();
        }
        _doneDraw = true;
    }

    private IEnumerator WaitForAnswer()
    {
        yield return new WaitWhile(() => answer == -1);
        Debug.Log("Done waiting");
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
                    Debug.Log("Draw Help");
                    DrawHelp();
                    return;
                }
            case TileTypes.Monster:
                {
                    Debug.Log("Draw Monster");
                    DrawMonster();
                    return;
                }
            case TileTypes.Weapon:
                {
                    Debug.Log("Draw Weapon");
                    DrawWeapon();
                    return;
                }
            default:
                {
                    Debug.Log("Draw None");
                    return;
                }
        }

    }

    /// <summary>
    /// Rest the varaibles to be ready for the next draw card.
    /// </summary>
    private void ResetVars()
    {
        answer = -1;
        _Piece = null;
        _CardType = TileTypes.Empty;
        DrawPanel.SetActive(false);
        _doneDraw = false;
    }

    private void DrawHelp()
    {
        //Draw the top card
        Card Top = _DeckHelp.Dequeue();

        //Add to the current pieces hand.
        Card removedCard = _Piece.AddCard(Top);
        if (removedCard)
            _DeckHelp.Enqueue(removedCard);
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

    /// <summary>
    /// Add each card into the respective decks in a random order.
    /// </summary>
    /// <param name="cardResources">resource array contain the loaded cards.</param>
    /// <param name="deck">deck for which the cards need to be place into.</param>
    private void RandomlyEnqueueCards(Card[] cardResources, Queue<Card> deck)
    {
        List<Card> tempDeck = new List<Card>();
        int currentDeckSize = cardResources.Length;
        int index;
        System.DateTime localDate = System.DateTime.Now;
        System.Random rand = new System.Random(localDate.Millisecond);

        // covernt the array over to list for easy removing of cards.
        foreach (Card card in cardResources)
        {
            tempDeck.Add(card);
        }

        // pick a random card from the resources and enqueue it to the deck.
        while (currentDeckSize > 0)
        {
            index = rand.Next(currentDeckSize);
            deck.Enqueue(tempDeck[index]);
            tempDeck.RemoveAt(index);
            currentDeckSize--;
        }
    }

    private void YesClicked()
    {
        answer = 1;
    }

    private void NoClicked()
    {
        answer = 0;
    }
}
