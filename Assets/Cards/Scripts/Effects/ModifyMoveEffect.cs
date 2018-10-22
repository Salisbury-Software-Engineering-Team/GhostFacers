using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ModifyMoveEffect")]
public class ModifyMoveEffect : Effect
{
    public int Movement = 0;

    public override void OnActivate(Card card)
    {

        if (CharacterOwner)
        {
            if (isStagged)
            {
                // Means card is ready to be used and not waiting for some phase to end.

                //card activated
                didActivate = true;
                CharacterOwner.GetComponent<Roll>().ModifyRoll(Movement); // Apply effect
                HandleWhenToDiscard(); // stage card for discard
            }
            else
            {
                Debug.Log("Error: OnActivation cardEffect not stagged for modifyMovement. Must be stagged before applied. Cant spell.");
            }
        }
        else
        {
            Debug.Log("Error, Card: \"" + Name + "\" does not have an ower when trying to activate.");
        }
    }

    /// <summary>
    /// Determines when to add and remove the card from stagged for use. 
    /// </summary>
    public override void ToggleActivation()
    {
        if (isStagged)
        {
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
