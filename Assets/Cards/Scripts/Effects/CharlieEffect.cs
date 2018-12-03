using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Charlie")]
public class CharlieEffect : Effect
{
    public int AmountToHeal = 0;
    public int numAttackDiceIncrease = 0;

    public override void InitializeEffectFunctions()
    {
        AttackEffectFunctions += () => Charlie();
        InstantEffectFunctions += () => HealUser();
    }

    protected override void SetDescription()
    {
        Description = "Add " + numAttackDiceIncrease + " Attack Dice and Heal Character for " + AmountToHeal + " Health.";
    }

    private void Charlie()
    {
        GameManager.instance.gameObject.GetComponent<Attack>().ModifyAttack(numAttackDiceIncrease); // Apply effect
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
