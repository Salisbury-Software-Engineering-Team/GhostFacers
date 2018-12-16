using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For each Deck created, Please creat a Handle"Name of the Deck"Discard() to handle what happens to that card
/// when it is discarded.
///     ex: HandleMonsterDiscard() {}
/// </summary>
public class Deck : MonoBehaviour
{
    private CharacterPiece _Piece;
    private TileTypes _CardType;
    private int answer = -1;
    private bool _doneDraw;

    private Queue<Card> _DeckMonster;
    private Queue<Card> _DeckHelp;
    private Queue<Card> _DeckWeapon;
    private Queue<Card> _DeckAngel;
    private Queue<Card> _DeckDaemon;
    private Queue<Card> _DeckSpecial;

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
        _DeckAngel = new Queue<Card>();
        _DeckDaemon = new Queue<Card>();
        _DeckSpecial = new Queue<Card>();

        // Create wepon deck
        Card[] weapons = Resources.LoadAll<Card>("Weapon/Playable");
        Card[] help = Resources.LoadAll<Card>("Help/Playable");
        Card[] monster = Resources.LoadAll<Card>("Monster/Playable");
        Card[] angel = Resources.LoadAll<Card>("Angel/Playable");
        Card[] daemon = Resources.LoadAll<Card>("Daemon/Playable");
        Card[] special = Resources.LoadAll<Card>("Special/Playable");

        RandomlyEnqueueCards(weapons, _DeckWeapon);
        RandomlyEnqueueCards(help, _DeckHelp);
        RandomlyEnqueueCards(monster, _DeckMonster);
        RandomlyEnqueueCards(angel, _DeckAngel);
        RandomlyEnqueueCards(daemon, _DeckDaemon);
        RandomlyEnqueueCards(special, _DeckSpecial);

        AddDiscardHandlers();
    }

    /// <summary>
    /// Add event handler for what happens to a discarded card
    /// </summary>
    private void AddDiscardHandlers()
    {
        foreach (Card card in _DeckMonster)
        {
            card.DiscardHandler += (c) => OnDiscard(c);
        }
        foreach (Card card in _DeckWeapon)
        {
            card.DiscardHandler += (c) => OnDiscard(c);
        }
        foreach (Card card in _DeckHelp)
        {
            card.DiscardHandler += (c) => OnDiscard(c);
        }
        foreach (Card card in _DeckAngel)
        {
            card.DiscardHandler += (c) => OnDiscard(c);
        }
        foreach (Card card in _DeckDaemon)
        {
            card.DiscardHandler += (c) => OnDiscard(c);
        }
        foreach (Card card in _DeckMonster)
        {
            card.DiscardHandler += (c) => OnDiscard(c);
        }
    }

    /// <summary>
    /// Begin the drawing of a card phase.
    /// </summary>
    /// <param name="piece">Current piece that is looking to draw a card</param>
    public void DrawCard(CharacterPiece piece)
    {
        StartCoroutine(DrawLoop(piece));
    }

    /// <summary>
    /// used to Draw a card from the passed deck type a give to the passed piece
    /// </summary>
    /// <param name="piece">Piece card is given to</param>
    /// <param name="type">Deck to draw from</param>
    public void DrawCard(CharacterPiece piece, CardType type)
    {
        StartCoroutine(DrawLoop(piece, type));
    }

    private IEnumerator DrawLoop(CharacterPiece piece)
    {
        ResetVars();
        _Piece = piece;
        _CardType = _Piece.CurrentTile.Type.Type;
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
                yield return DetermineDeck();
        }
        _doneDraw = true;
    }

    /// <summary>
    /// Use this to draw a card from a deck and give it to the passed piece.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator DrawLoop(CharacterPiece piece, CardType type)
    {
        _Piece = piece;
        _CardType = (TileTypes)type;
        if (CanPieceDraw())
        {
            Debug.Log(_Piece + " can Draw.");
            //user said yes
            answer = 1;
            if (answer == 1)
                yield return DetermineDeck();
        }
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

    private IEnumerator DetermineDeck()
    {
        switch (_CardType)
        {
            case TileTypes.Help:
                {
                    Debug.Log("Draw Help");
                    DrawHelp();
                    break;
                }
            case TileTypes.Monster:
                {
                    Debug.Log("Draw Monster");
                    DrawMonster();
                    break;
                }
            case TileTypes.Weapon:
                {
                    Debug.Log("Draw Weapon");
                    DrawWeapon();
                    break;
                }
            case TileTypes.Angel:
                {
                    Debug.Log("Draw Angel");
                    DrawAngel();
                    break;
                }
            case TileTypes.Daemon:
                {
                    Debug.Log("Draw Daemon");
                    DrawDaemon();
                    break;
                }
            case TileTypes.SpecialWeapon:
                {
                    Debug.Log("Draw Special");
                    DrawSpecial();
                    break;
                }
            default:
                {
                    Debug.Log("Draw None");
                    break;
                }
        }

        // wait and display the drawn card
        yield return DisplayDrawnCard();

    }

    private IEnumerator DisplayDrawnCard()
    {
        yield return new WaitForSeconds(4);
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
        Debug.Log("Drawing a Help Card. Card = " + Top.Name);

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

    private void DrawAngel()
    {
        //Draw the top card
        Card Top = _DeckAngel.Dequeue();

        //Add to the current pieces hand.
        Card removedCard = _Piece.AddCard(Top);
        if (removedCard)
            _DeckAngel.Enqueue(removedCard);
    }

    private void DrawDaemon()
    {
        //Draw the top card
        Card Top = _DeckDaemon.Dequeue();

        //Add to the current pieces hand.
        Card removedCard = _Piece.AddCard(Top);
        if (removedCard)
            _DeckDaemon.Enqueue(removedCard);
    }

    private void DrawSpecial()
    {
        //Draw the top card
        Card Top = _DeckSpecial.Dequeue();

        //Add to the current pieces hand.
        Card removedCard = _Piece.AddCard(Top);
        if (removedCard)
            _DeckSpecial.Enqueue(removedCard);
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
            card.Initialize();
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

    /// <summary>
    /// Card that is being discarded is added back to the deck.
    /// </summary>
    /// <param name="c">card to be added back into the deck</param>
    public void OnDiscard(Card c)
    {
        Debug.Log("OnDiscard Called");
        //TODO Handle how a card is discarded.

        // Calls the effect OnDiscard() to determine if anything needs to happen
        // with the effect when the card is being discarded.
        //if (c)
        //    c.OnDiscard();

        // Loop thru the different deck types and determine with deck to put the card.
        foreach (CardType type in Enum.GetValues(typeof(CardType)))
        {
            if (type == c.DeckType)
            {
                // Dynamicly get the discard funtion name by using the deck type.
                string methodName = "Handle" + type.ToString() + "Discard";
                //Debug.Log("Card = " + c.Name + "Discard Method Name = " + methodName);

                try
                {
                    // Call proper discard function
                    MethodInfo mi = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance); 
                    mi.Invoke(this, new object[] {c});
                }
                catch (NullReferenceException e)
                {
                    Debug.Log("Error on discarding card. No funcation found to handle discard\n"+e);
                }
            }
        }

        Debug.Log("Help " + _DeckHelp);

    }

    private void HandleMonsterDiscard(Card c)
    {
        Debug.Log("HandleMonsterDiscard Called");
        _DeckMonster.Enqueue(c);
    }

    /// <summary>
    /// Places the Discarded HElp Card back into the Deck.
    /// </summary>
    /// <param name="c"></param>
    private void HandleHelpDiscard(Card c)
    {
        Debug.Log("HandleHelpDiscard Called");
        _DeckHelp.Enqueue(c);
    }

    /// <summary>
    /// Places the Discarded Weapon card at the end of the deck.
    /// </summary>
    /// <param name="c"></param>
    private void HandleWeaponDiscard(Card c)
    {
        Debug.Log("HandleWeaponDiscard Called");
        _DeckWeapon.Enqueue(c);
    }

    private void HandleAngelDiscard(Card c)
    {
        Debug.Log("HandleAngelDiscard Called");
        _DeckAngel.Enqueue(c);
    }

    private void HandleDaemonDiscard(Card c)
    {
        Debug.Log("HandleDaemonDiscard Called");
        _DeckDaemon.Enqueue(c);
    }

    private void HandleSpecialDiscard(Card c)
    {
        Debug.Log("HandSpecialpDiscard Called");
        _DeckSpecial.Enqueue(c);
    }
}
