using System.Collections.Generic;
using System;
using UnityEngine;

//make an enum variable for when card effect can be played (can also be summonable) (look in turnManager file for phases)

//[Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public bool Summonable; //used for displaying card

    public string Name;
    public int Health;
    public int Attack;

    //Inventories (only used for summonable cards)
    private int Help; //for humans
    private int Weapon; //for humans
    private int Inventory; //for everyone but humans

    public Sprite artwork;

    public string Description;
    //for humans, says number of weapons and help character can hold
    //for other cards, says their effect or is left blank
    //non-humans can hold up to 2 cards by default

    //What phase the card effect can be used in (None for non effect cards)
    [SerializeField] private Phase _EffectPhase;
    public Phase EffectPhase
    {
        get { return _EffectPhase; }
    }

    //used for summonable cards
    public Card(string N, int H, int A, string D, Phase E) { Name = N; Health = H; Attack = A; Description = D; Summonable = true; Inventory = 2; _EffectPhase = E; }
    //used for effect cards that can't be summoned
    public Card(string N, string D, Phase E) { Name = N; Description = D; Summonable = false; _EffectPhase = E; }

}

/*
Effect:
    Name
    Picture
    Description

Summonable:
    Name
    Picture
    Health and Attack
    Description //can be blank if not human and no effect

    Example:
        Death
        //Picture
        Health = 8 Attack = 4
        Cannot take damage by normal attacks

        Dean
        //Picture
        Health = 16 Attack = 3
        3 Weapon 1 Help

        Werewolf
        //Picture
        Health = 4 Attack = 1
 */
 //Green Angel
 //Green Monster

    //angels can hold weapons and help
    //monsters can hold monsters
    //demons can hold demons
    //humans can hold help and weapons