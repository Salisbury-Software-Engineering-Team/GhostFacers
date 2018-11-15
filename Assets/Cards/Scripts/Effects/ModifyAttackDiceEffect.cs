using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ModifyAttackDice")]
public class ModifyAttackDiceEffect : Effect
{
    public int numAttackDiceIncrease = 0;
    public CharacterStat SpecialCharacter; // If held by bobby, gets better increase.
    public int numAttackDiceIncreaseBobby = 0;
    
    public override void OnActivate(Card card)
    {
        if (CharacterOwner)
        {
            if (isStagged)
            {
                // Means card is ready to be used and not waiting for some phase to end.

                //card activated
                // TODO: Change attack to be differnet for each character not attached to gamemanager.
                didActivate = true;
                if (CharacterOwner.Stat == SpecialCharacter)
                    GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncreaseBobby); // Apply effect for BObby
                else
                    GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncrease); // Apply effect
                HandleWhenToDiscard(); // stage card for discard
            }
            else
            {
                Debug.Log("Error: OnActivation cardEffect not stagged for modifyAttackDice. Must be stagged before applied. Cant spell.");
            }
            
        }
        else
        {
            Debug.Log("Error, Card: \"" + Name + "\" does not have an ower when trying to activate.");
        }
    }

    public override void ToggleActivation()
    {
        if (isStagged)
        {
            // User decieded to not use the current card
            isStagged = false;
            CharacterOwner.RmFromStaggedForCurrentPhase(card);
        }
        else
        {
            // Means the card has to wait for something before activating, but the use has selected
            // this card for use. EX: Increase roll amount card has been selected to be used, but roll button
            // has not been pressed yet. This would allow the user to change his mind about the card use.
            isStagged = true;
            CharacterOwner.AddToStaggedForCurrentPhase(card);
            Debug.Log("Card " + card.Name + " Added to stagged");
        }
    }
}
