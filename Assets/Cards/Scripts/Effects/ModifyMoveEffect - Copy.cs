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
            CharacterOwner.GetComponent<Roll>().ModifyRoll(Movement);
            HandleWhenToDiscard();
        }
        else
        {
            Debug.Log("Error, Card: \"" + Name + "\" does not have an ower when trying to activate.");
        }
    }
}
