using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ModifyAttackDice")]
public class ModifyAttackDiceEffect : Effect
{
    public int numAttackDiceIncrease = 0;
    public CharacterStat Bobby; // If held by bobby, gets better increase.
    public int numAttackDiceIncreaseBobby = 0;

    public override void OnActivate(Card card)
    {
        if (CharacterOwner)
        {
            if (CharacterOwner.Stat == Bobby)
            {

            }
            HandleWhenToDiscard();
        }
        else
        {
            Debug.Log("Error, Card: \"" + Name + "\" does not have an ower when trying to activate.");
        }
    }
}
