using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Effects/Choice")]
public class ChoiceEffect : Effect
{
    [SerializeField] private List<Effect> effects;
    [HideInInspector] public List<Effect> InstanceEffects;

    public override void Initialize(Card c)
    {
        InstanceEffects = new List<Effect>();
        for (int i = 0; i < effects.Count; i++)
        {
            InstanceEffects.Add(Instantiate(effects[i]));
            InstanceEffects[i].Initialize(null);
        }
        base.Initialize(c);
    }

    public override void InitializeEffectFunctions()
    {
        for (int i = 0; i < InstanceEffects.Count; i++)
        {
            InstanceEffects[i].InitializeEffectFunctions();
        }
    }

    public override void OnDraw(CharacterPiece piece)
    {
        base.OnDraw(piece);
        for (int i = 0; i < InstanceEffects.Count; i++)
        {
            InstanceEffects[i].OnDraw(piece);
        }
    }

    public override void OnDiscard()
    {
        base.OnDiscard();
        for (int i = 0; i < InstanceEffects.Count; i++)
        {
            InstanceEffects[i].OnDiscard();
        }
    }

    public override void ToggleActivation()
    {
        DisplayChoices();
    }

    protected override void SetDescription()
    {
        //throw new System.NotImplementedException();
    }

    private void DisplayChoices()
    {
        GameManager ins = GameManager.instance;
        ins.ChoiceEffect = card;
    }

    public void EffectPicked(Effect e)
    {
        e.ToggleActivation();
        ReadyToDiscard();
    }

}
