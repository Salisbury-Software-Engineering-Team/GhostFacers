using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ModifyAttackDice")]
public class ModifyAttackDiceEffect : Effect
{
    public int numAttackDiceIncrease = 0;
    public CharacterStat SpecialCharacter; // If held by bobby, gets better increase.
    public int numAttackDiceIncreaseBobby = 0;

    public override void InitializeEffectFunctions()
    {
        AttackEffectFunctions += () => IncreaseAttack();
    }

    public override void OnDraw(CharacterPiece piece)
    {
        throw new System.NotImplementedException();
    }

    private void IncreaseAttack()
    {
        if (CharacterOwner.Stat == SpecialCharacter)
            GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncreaseBobby); // Apply effect for BObby
        else
            GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncrease); // Apply effect

    }
}
