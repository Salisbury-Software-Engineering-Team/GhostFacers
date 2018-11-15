using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ModifyAttackDice")]
public class ModifyAttackDiceEffect : Effect
{
    public int numAttackDiceIncrease = 0;
    public CharacterStat SpecialCharacter; // If held by bobby, gets better increase.
    public int numSpecialAttackDiceIncrease = 0;

    public override void InitializeEffectFunctions()
    {
        AttackEffectFunctions += () => IncreaseAttackDice();
    }

    private void IncreaseAttackDice()
    {
        if (CharacterOwner.Stat == SpecialCharacter)
            GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numSpecialAttackDiceIncrease); // Apply effect for BObby
        else
            GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncrease); // Apply effect

    }
}
