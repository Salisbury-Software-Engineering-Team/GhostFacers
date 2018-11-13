using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Charlie")]
public class CharlieEffect : Effect
{
    public int AmountToHeal = 0;
    public int numAttackDiceIncrease = 0;

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

    /// <summary>
    /// Charlies Card can not be unselected. Once used the card effect is applied.
    /// </summary>
    public override void ToggleActivation()
    {
        isStagged = true;
        HealUser();
        CharacterOwner.AddToStaggedForCurrentPhase(card);
        Debug.Log("Card " + card.Name + " Added to stagged");
    }

    /// <summary>
    /// First part of card effect heals the user.
    /// </summary>
    private void HealUser()
    {
        if (CharacterOwner)
            CharacterOwner.Heal(AmountToHeal);
    }
}
