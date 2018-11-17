using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ModifyMoveEffect")]
public class ModifyMoveEffect : Effect
{
    public int Movement = 0;

    public override void InitializeEffectFunctions()
    {
        RollEffectFunctions += () => Move();
    }

    private void Move()
    {
        Debug.Log("ModifyMovementEffect Applied. Roll increase: " + Movement);
        CharacterOwner.GetComponent<Roll>().ModifyRoll(Movement); // Apply effect
    }
}
