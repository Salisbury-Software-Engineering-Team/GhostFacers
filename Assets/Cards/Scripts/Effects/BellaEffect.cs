using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bella")]
public class BellaEffect : Effect
{
    public int numAttackDiceIncrease = 0;
    public int amountOfCardsToDraw = 0;
    public CardType deckToDrawFrom = CardType.None;
    private GameManager inst;

    public override void InitializeEffectFunctions()
    {
        inst = GameManager.instance;
        AttackEffectFunctions += () => AddAttackDice();
        InstantEffectFunctions += () => DrawCards();
    }

    protected override void SetDescription()
    {
        Description = "Add " + numAttackDiceIncrease + " Attack Dice and Draw " + amountOfCardsToDraw + " " + deckToDrawFrom + " Cards.";
    }

    private void AddAttackDice()
    {
        GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncrease); // Apply effect
    }

    private void DrawCards()
    {
        for (int i = 0; i < amountOfCardsToDraw; i++)
        {
            inst.CardDeck.DrawCard(CharacterOwner, deckToDrawFrom);
        }
    }
}
