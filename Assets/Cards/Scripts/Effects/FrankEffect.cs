using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Frank")]
public class FrankEffect : ChoiceEffect
{
    public int amountToKill = 1;
    public PieceType typeToKill = PieceType.Leviathan;


    public override void InitializeEffectFunctions()
    {
        
    }

    protected override void SetDescription()
    {
        Description = "Kill one leviathan anywhere on board or add 4 to movement";
    }
}
