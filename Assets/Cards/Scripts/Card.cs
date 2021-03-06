﻿using System.Collections.Generic;
using System;
using UnityEngine;

//make an enum variable for when card effect can be played (can also be summonable) (look in turnManager file for phases

//[Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public bool Summonable; //used for displaying card

    public Effect CardEffect;

    public bool DidActivate { get { return CardEffect.didActivate; } }
    public bool IsStagged { get { return CardEffect.isStagged; } }
    public bool canToggle { get { return CardEffect.canToggle; } }

    public string Name;
    public Sprite artwork;
    public Sprite backImage;
    [HideInInspector] public CharacterPiece CharacterOwner;

    //For humans, says number of weapons and help character can hold
    //for other cards, says their effect or is left blank
    //non-humans can hold up to 2 cards by default
    public string Description;

    [SerializeField] public CardType DeckType;
    public CharacterStat Stat;
    public event Action<Card> DiscardHandler;

    public void Awake()
    {
        Debug.Log("Awake Card Called");
        if (CardEffect)
        {
            Name = CardEffect.Name;
            Description = CardEffect.Description;
        }
    }

    /// <summary>
    /// Returns the phase that the card can be activated in. EX: Attack, Movement, Roll, etc...
    /// </summary>
    public List<Phase> EffectPhases
    {
        get {
            if (CardEffect)
                return CardEffect.ActivatePhases;
            else
            {
                Debug.Log("Error Card.EffectPhase " + Name + " : Does not have a CardEffect.");
                return null;
            }
        }
    }

    //used for summonable cards
    public Card(string N, int H, int A, string D)
    {
        Name = N;
        Stat.CurrentHealth = H;
        Stat.CurrentAttack = A;
        Description = D;
        Summonable = true;
    }

    /// <summary>
    /// Usally callled at the start of a new game. This will initialize variables
    /// for the card and allow it to be activated. 
    /// </summary>
    public void Initialize()
    {
        if (CardEffect)
        {
            CardEffect.Initialize(this);
            CardEffect.InitializeEffectFunctions();
            Name = CardEffect.Name;
            Description = CardEffect.Description;
        }
        

    }

    /// <summary>
    /// Called when card is being discarded. My not happen when card is used. Usally will be stagged
    /// for discard and called at end of phase. Card is also flagged as not activated for if card can be used again.
    /// </summary>
    public void OnDiscard()
    {
        //handle discard 
        if (CardEffect)
            CardEffect.OnDiscard();
        DiscardHandler.Invoke(this);
        CharacterOwner = null;
    }

    /// <summary>
    /// Called when the card is drawn from the deck. 
    /// </summary>
    /// <param name="c">Character that drew the card. (Owner)</param>
    public void OnDraw(CharacterPiece piece)
    {
        CharacterOwner = piece;
        if (CardEffect)
        {
            CardEffect.OnDraw(piece);
            Debug.Log("OnDraw owner = " + piece.Stat.Name);
        }
    }
    /// <summary>
    /// Called when card is being used.
    /// </summary>
    public void OnActivate()
    {
        
    }

    public void ToggleActiavation()
    {
        Debug.Log("Card " + Name + " ToggleActivationCalled");
        CardEffect.ToggleActivation();
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