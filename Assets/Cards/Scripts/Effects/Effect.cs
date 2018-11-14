using System;
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

    [SerializeField] protected int numUsesLeft = 1; // how many times the card can be used. -1 for unlimited, 

    protected Action AttackEffectFunctions; // Used to apply effect of the card at next attack
    protected Action DrawEffectFunctions; // Used to apply effect of the card at next draw
    protected Action RollEffectFunctions; // Used to apply effect of the card at next roll
    protected Action EndEffectFunctions; // Used to apply effect of the card on next end turn
    protected Action InstantEffectFunctions; // Used to aply effect that happen when card activate button is pressed.
    protected Action DiscardEffectFunction; // What happens when a card is discarded. Can be used to turn off active effects

    // Called when card is created
    public virtual void Initialize(Card c)
    {
        card = c;
        CharacterOwner = null;
        didActivate = false;
        isStagged = false;
    }

    /// <summary>
    /// Add any functions here that will be used to apply any effect that is card will have.
    /// 
    /// Add the functions you create in the child class with "AttackEffectFunctions += () => "name of function"();"
    /// Look at the current effect child classes for an example. Having the effects applied this way allows multiple 
    /// effects to be aplied by one card at different phases. 
    /// </summary>
    public abstract void InitializeEffectFunctions();

    // Called when the effect is triggered.
    public virtual void OnActivate()
    {
        if (CharacterOwner)
        {
            if (isStagged)
            {
                // Means card is ready to be used and not waiting for some phase to end.

                //card activated
                // TODO: Change attack to be differnet for each character not attached to gamemanager.
                didActivate = true;
                DetermineWhichEffectToInvoke();
            }
            else
            {
                Debug.Log("Error: OnActivation cardEffect not stagged for modifyAttackDice. Must be stagged before applied. Cant spell.");
            }

        }
        else
        {
            Debug.Log("Error, Card: \"" + Name + "\" does not have an ower when trying to activate.");
        }
    }

    /// <summary>
    /// Adds all effects of this card into the proper action delegate for the character. eg: if the card has multiple effects 
    /// that happen for different phases, then this will add them to each of the proper stagged actions.
    /// </summary>
    private void AddEffectToProperPhase()
    {
        if (AttackEffectFunctions != null)
        {
            CharacterOwner.StaggedForAttackPhase += () => card.OnActivate();
        }

        if (RollEffectFunctions != null)
        {
            CharacterOwner.StaggedForRollPhase += () => card.OnActivate();
        }

        if (EndEffectFunctions != null)
        {
            CharacterOwner.StaggedForEndPhase += () => card.OnActivate();
        }
    }

    private void RemoveEffectFromProperPhase()
    {
        if (AttackEffectFunctions != null)
        {
            CharacterOwner.StaggedForAttackPhase -= () => card.OnActivate();
        }

        if (RollEffectFunctions != null)
        {
            CharacterOwner.StaggedForRollPhase -= () => card.OnActivate();
        }

        if (EndEffectFunctions != null)
        {
            CharacterOwner.StaggedForEndPhase -= () => card.OnActivate();
        }
    }

    /// <summary>
    /// Determines what effect should be applied baised off the current phase.
    /// </summary>
    private void DetermineWhichEffectToInvoke()
    {
        switch (GameManager.instance.TurnPhase)
        {
            case Phase.Attack:
                {
                    CharacterOwner.StaggedForAttackPhase -= () => card.OnActivate();

                    if (AttackEffectFunctions != null)
                    {
                        if (numUsesLeft <= 0)
                            numUsesLeft--;
                        AttackEffectFunctions.Invoke();
                    }

                    if (numUsesLeft == 0)
                        ReadyToDiscard(); // stage card for discard
                    break;
                }
            case Phase.Roll:
                {
                    CharacterOwner.StaggedForRollPhase -= () => card.OnActivate();

                    if (RollEffectFunctions != null)
                    {
                        if (numUsesLeft <= 0)
                            numUsesLeft--;
                        RollEffectFunctions.Invoke();
                    }

                    if (numUsesLeft == 0)
                        ReadyToDiscard(); // stage card for discard
                    break;
                }
            case Phase.EndTurn:
                {
                    CharacterOwner.StaggedForEndPhase -= () => card.OnActivate();

                    if (EndEffectFunctions != null)
                    {
                        if (numUsesLeft <= 0)
                            numUsesLeft--;
                        EndEffectFunctions.Invoke();
                    }

                    if (numUsesLeft == 0)
                        ReadyToDiscard(); // stage card for discard
                    break;
                }
            default:
                {
                    Debug.Log("Error: No Action found for current phase: " + GameManager.instance.TurnPhase);
                    break;
                }
        }

    }

    /// <summary>
    /// This should be called when the use buton is pressed. Handle how the effect of the card is applied and 
    /// when to apply the effect. Some card, like increase roll must wait till roll button is presed. These 
    /// cards will be stagged for activation. Other card can just have the effect applied right away. Handle
    /// in the child class.
    /// </summary>
    public virtual void ToggleActivation()
    {
        if (canToggle)
        {
            if (isStagged)
            {
                // User decieded to not use the current card
                isStagged = false;
                RemoveEffectFromProperPhase();
            }
            else
            {
                // Means the card has to wait for something before activating, but the use has selected
                // this card for use. EX: Increase roll amount card has been selected to be used, but roll button
                // has not been pressed yet. This would allow the user to change his mind about the card use.
                isStagged = true;
                AddEffectToProperPhase();
                Debug.Log("Card " + card.Name + " Added to stagged");
            }
        }
        else
        {
            isStagged = true;
            AddEffectToProperPhase();
            InstantEffectFunctions.Invoke();
            Debug.Log("Card " + card.Name + " Added to stagged");
        }
    }

    public void SetOwner(CharacterPiece piece)
    {
        CharacterOwner = piece;
    }

    // Determine if anything is applied 
    public virtual void OnDraw(CharacterPiece piece)
    {
        if (DrawEffectFunctions != null)
        {
            DrawEffectFunctions.Invoke();
        }
    }

    /// <summary>
    /// Used if anything happens when the card is discarded.
    /// </summary>
    public virtual void OnDiscard()
    {
        didActivate = false;
        isStagged = false;
        CharacterOwner = null;
        if (DiscardEffectFunction != null)
            DiscardEffectFunction.Invoke();
    }

    // This is to handle when a card should be discarded.
    //**************Must be called for each Effect Created**********************.
    protected virtual void ReadyToDiscard()
    {
        CharacterOwner.AddToStaggedForDiscard(card);
    }
}
