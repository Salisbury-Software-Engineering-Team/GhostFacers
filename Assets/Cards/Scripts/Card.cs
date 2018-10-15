using System.Collections.Generic;
using System;
using UnityEngine;

//make an enum variable for when card effect can be played (can also be summonable) (look in turnManager file for phases

//[Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public bool Summonable; //used for displaying card

    public Effect CardEffect;

    [SerializeField] private bool _didActivate = false;
    public bool DidActivate { get { return _didActivate; } }

    public string Name;
    /*
    public int Health;
    public int Attack;

    //Inventories (only used for summonable cards)
    public int Help; //for humans
    public int Weapon; //for humans
    public int Inventory; //for everyone but humans
    */

    public Sprite artwork;
    public Sprite backImage;
    //Hi
    public string Description;
    //for humans, says number of weapons and help character can hold
    //for other cards, says their effect or is left blank
    //non-humans can hold up to 2 cards by default

    [SerializeField] public CardType DeckType;
    private readonly Deck deck; // reference to the deck
    public CharacterStat Stat;
    public event Action<Card> DiscardHandler;

    // Return the phase that the card can be activated.
    public Phase EffectPhase
    {
        get {
            if (CardEffect)
                return CardEffect.ActivatePhase;
            else
            {
                Debug.Log("Error Card.EffectPhase " + Name + " : Does not have a CardEffect.");
                return Phase.None;
            }
        }
    }

    //used for summonable cards
    public Card(string N, int H, int A, string D)
    {
        Name = N;
        Stat.Health = H;
        Stat.Attack = A;
        Description = D;
        Summonable = true;
    }

    // Set up vars for a new deck.
    public void Initialize()
    {
        _didActivate = false;

    }

    /// <summary>
    /// 
    /// </summary>
    public void OnDiscard()
    {
        /// TODO: handle what happens to a discarded card. Put back in deck.
        DiscardHandler.Invoke(this);
        _didActivate = false;
    }

    public void OnDraw(CharacterPiece c)
    {
        if (CardEffect)
        {
            CardEffect.CharacterOwner = c;
        }
    }
    /// <summary>
    /// Called when card is being used.
    /// </summary>
    public void OnActivate()
    {
        if (CardEffect)
        {
            Debug.Log("Card " + Name + " Activated");
            _didActivate = true;
            CardEffect.OnActivate();
        }
        else
            Debug.Log("Error Card.OnActivate(): No Effect Found to card.");
    }
}

//angel red 255 0 0
//monster green 0 255 0
//demon orange 173 96 0
//help yellow 255 255 0
//weapon purple 255 0 255

    //angels can hold weapons and help
    //monsters can hold monsters
    //demons can hold demons
    //humans can hold help and weapons