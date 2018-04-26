using System.Collections.Generic;
using System;
using UnityEngine;

//[Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{

    // needs changing
    public string Name;
    public int Health;
    public int Attack;

    public Sprite artwork;

    public string Description; 
    //for humans, says number of weapons and help character can hold
    //for other cards, says their effect or is left blank
    //non-humans can hold up to 2 cards by default

    //used for summonable cards
    public Card(string N, int H, int A, string D) { Name = N; Health = H; Attack = A; Description = D; }
    //used for effect cards that can't be summoned
    public Card(string N, string D) { Name = N; Description = D; }

}
