using System.Collections;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    // How many more turn the effect should continued to be applied to the piece.
    // We could use -1 to mean infinite.
    public int TurnsLeft = 0;

    public string Name = "New Effect"; // Name of the effect
    public string Description = "Enter Description"; // Description of what the effect does.
    public Phase ActivatePhase; // phase that the card can be played.
    protected CharacterPiece CharacterOwner;
    protected Card card;
    public bool didActivate; // card effect has been activated.
    public bool isStagged; // card effect is waiting to be activated.
    public bool canToggle = true; // determines if the card can be unselected before activation.

    // Called when card is created
    public virtual void Initialize(Card c)
    {
        card = c;
        CharacterOwner = null;
        didActivate = false;
        isStagged = false;
    }

    // Called when the effect is triggered.
    public abstract void OnActivate(Card card);

    /// <summary>
    /// This should be called when the use buton is pressed. Handle how the effect of the card is applied and 
    /// when to apply the effect. Some card, like increase roll must wait till roll button is presed. These 
    /// cards will be stagged for activation. Other card can just have the effect applied right away. Handle
    /// in the child class.
    /// </summary>
    public abstract void ToggleActivation();

    // Determine if anything is applied 
    public virtual void OnDraw(CharacterPiece piece)
    {
        CharacterOwner = piece;

    }

    // Used if anything happens when the card is discarded.
    public virtual void OnDiscard()
    {
        didActivate = false;
        isStagged = false;
        CharacterOwner = null;
    }

    // This is to handle when a card should be discarded.
    //**************Must be called for each Effect Created**********************.
    protected virtual void HandleWhenToDiscard()
    {
        CharacterOwner.AddToStaggedForDiscard(card);
    }


    public void CardActivatedButNotUsed()
    {

    }

    /// <summary>
    /// User has decided to use the card, but the card to not yet ready to be played. 
    /// </summary>
    public virtual void AddToStaggedForCurrentPhase()
    {
        isStagged = true;
    }

    public virtual void RmFromStaggedForCurrentPhase()
    {
        isStagged = false;
    }


}
