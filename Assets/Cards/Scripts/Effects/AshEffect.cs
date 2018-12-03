using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Ash")]
//Move user anywhere on board
//set movement = 100 and go to move phase
public class Ash : Effect
{
    //Character piece
    [SerializeField] private CharacterPiece _piece;

    public override void InitializeEffectFunctions()
    {
        InstantEffectFunctions += () => MoveAnywhere();
    }

    protected override void SetDescription()
    {
        Description = "Move the user anywhere on the board";
    }

    private void MoveAnywhere()
    {
        _piece = GameManager.instance.CurrentPiece;
        _piece.DisplayAvaliableMovement(100); // display all spaces on board for user to select
    }
}