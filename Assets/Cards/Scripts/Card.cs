using System.Collections.Generic;
using System;
using UnityEngine;

//make an enum variable for when card effect can be played (can also be summonable) (look in turnManager file for phases)

public enum cardType
{
    None,
    Help,
    Weapon,
    Angel,
    Demon,
    Monster,
}

//[Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public bool Summonable; //used for displaying card

    public string Name;
    public string Health;
    public string Attack;

    //Inventories (only used for summonable cards)
    public int Help; //for humans
    public int Weapon; //for humans
    public int Inventory; //for everyone but humans

    public Sprite artwork;
    public Sprite backImage;

    public string Description;
    //for humans, says number of weapons and help character can hold
    //for other cards, says their effect or is left blank
    //non-humans can hold up to 2 cards by default

    [SerializeField] public cardType Deck;

    //What phase the card effect can be used in (None for non effect cards)
    [SerializeField] private Phase _EffectPhase;
    public Phase EffectPhase
    {
        get { return _EffectPhase; }
    }

    //used for summonable cards
    public Card(string N, string H, string A, string D, Phase E) { Name = N; Health = H; Attack = A; Description = D; Summonable = true; Inventory = 2; _EffectPhase = E; }
    //used for effect cards that can't be summoned
    //public Card(string N, string D, Phase E) { Name = N; Description = D; Summonable = false; _EffectPhase = E; }

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