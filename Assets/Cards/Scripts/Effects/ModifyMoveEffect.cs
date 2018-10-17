using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Once card is activated. Start a coroutine that will wait until the phase moves to the movement phase an then add to the movement
/// </summary>

[CreateAssetMenu(menuName = "Effects/ModifyMoveEffect")]
public class ModifyMoveEffect : Effect
{
    public int Movement = 0;

    public override void OnActivate(Card card)
    {
        if (CharacterOwner)
        {
            CharacterOwner.GetComponent<Roll>().ModifyRoll(Movement);
            HandleWhenToDiscard();
        }
        else
        {
            Debug.Log("Error, Card: \"" + Name + "\" does not have an ower when trying to activate.");
        }
    }
}
