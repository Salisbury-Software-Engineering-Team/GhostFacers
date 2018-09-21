using System.Collections;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    // TODO: Add pahse variable that will be used to determine when the effect is appiled
    // This variable will mainly be used to effect that last mutiple turns.

    // how many more turn the effect should continued to be applied to the piece.
    // when could use -1 to mean infinite.
    public int TurnsLeft;
    public string Name = "New Effect"; // Name of the effect
    public string Description = "Enter Description"; // Description of what the effect does.
    public Phase ActivatePhase; // phase that the card can be played.

    // Called when card is created
    public abstract void Initialize(GameObject obj);

    // Called when the effect is triggered.
    public abstract void OnActivate();

    // Determine if anything is applied 
    public virtual void OnDraw() { }

    // Used if anything happens when the card is discarded.
    public virtual void OnDiscard() { }

}
