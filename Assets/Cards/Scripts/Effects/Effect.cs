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
    public CharacterPiece CharacterOwner;
    private Card card;

    // Called when card is created
    public virtual void Initialize(Card c)
    {
        card = c;
        CharacterOwner = null;
    }

    // Called when the effect is triggered.
    public abstract void OnActivate(Card card);

    // Determine if anything is applied 
    public virtual void OnDraw() { }

    // Used if anything happens when the card is discarded.
    public virtual void OnDiscard() { }

    // This is to handle when a card should be discarded.
    //**************Must be called for each Effect Created**********************.
    protected virtual void HandleWhenToDiscard()
    {
        CharacterOwner.AddToStaggedForDiscard(card);
    }


    public void CardActivatedButNotUsed()
    {

    }


}
