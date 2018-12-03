using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Roll 3 attack dice and heal the user by the damage dealt by the roll. If used by Ellen or Dean, fully heal the user
[CreateAssetMenu(menuName = "Effects/Jo")]
public class JoEffect : Effect
{
    private enum AttackDice
    {
        Empty = 0,
        Miss_1 = 1,
        Miss_2 = 2,
        Hit_1 = 3,
        Hit_2 = 4,
        Human = 5,
        Monster = 6,
    }

    public int AmountToHeal = 0;
    public CharacterStat SpecialCharacter; // If held by bobby, gets better increase.

    [SerializeField] private GameObject AttackDiceUI;
    [SerializeField] private GameObject AttackDicePanel;
    [SerializeField] private GameObject AttackDicePrefab;
    [SerializeField] private Text TotalHealText;
    [SerializeField] private Text _AttackPieceText;
    [SerializeField] private Text _HelpText;

    [SerializeField] private List<AttackDice> _AttackDiceList;
    [SerializeField] private int _AttackAmount;

    private bool _DoneAttackRoll;
    [SerializeField] private bool _AppliedHeal;

    public override void InitializeEffectFunctions()
    {
        InstantEffectFunctions += () => HealUser();
    }

    private void Init()
    {
        _DoneAttackRoll = false;
        _AttackAmount = 3;
        TotalHealText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (AttackDiceUI.activeInHierarchy)
            DisplayAttackDice();
        if (_DoneAttackRoll && !_AppliedHeal)
            ApplyHeal();
    }

    private void HealUser()
    {
        if (CharacterOwner.Stat == SpecialCharacter)
            CharacterOwner.Heal(CharacterOwner.Stat.StartHealth - CharacterOwner.Stat.CurrentHealth); //fully heal (max health - current health
        else
        {
            //Roll 3 attack dice and heal the damage
            CharacterOwner.Heal(AmountToHeal);
        }

    }

    public void RollAttackDice()
    {
        _AttackDiceList.Clear();
        for (int i = 0; i < _AttackAmount; i++)
        {
            int size = Enum.GetNames(typeof(AttackDice)).Length;
            int randomSelection = UnityEngine.Random.Range(1, size);
            _AttackDiceList.Add((AttackDice)randomSelection);
        }
        _DoneAttackRoll = true;
    }

    private void ApplyHeal()
    {
        int amountOfHealthRestored = 0;
        foreach (AttackDice dice in _AttackDiceList)
        {
            if (dice == AttackDice.Hit_1 || dice == AttackDice.Hit_2) // hit applied for humans and monsters
            {
                amountOfHealthRestored++;
                CharacterOwner.Heal(1);
            }
            else if (dice == AttackDice.Human && (CharacterOwner.Stat.Type == PieceType.Human || CharacterOwner.Stat.Type == PieceType.Angel))
            {
                // rolled human and piece attacking is human
                amountOfHealthRestored++;
                CharacterOwner.Heal(1);
            }
            else if (dice == AttackDice.Monster && CharacterOwner.Stat.Type != PieceType.Human && CharacterOwner.Stat.Type != PieceType.Angel)
            {
                //rolled dice is monster and piece attacking is monster, then hit piece
                amountOfHealthRestored++;
                CharacterOwner.Heal(1);
            }
        }
        _AppliedHeal = true;
        TotalHealText.text = "Total Health Restored: " + amountOfHealthRestored;
    }

    private void EndAttack()
    {
        _DoneAttackRoll = false;
        _AttackDiceList.Clear();
        _AttackAmount = 0;
    }

    private void DisplayAttackDice()
    {
        if (_HelpText) // display help message to roll dice
            _HelpText.text = "Roll to Heal";
        int currentAmountOfDice = AttackDicePanel.transform.childCount;

        // need to display more of lesss dice
        if (currentAmountOfDice != _AttackAmount)
        {
            if (currentAmountOfDice < _AttackAmount) // need to add more dice
            {
                for (int i = currentAmountOfDice; i < _AttackAmount; i++)
                {
                    Instantiate(AttackDicePrefab, AttackDicePanel.transform);
                }
            }
            else // need less dice
            {
                for (int i = 0; i < currentAmountOfDice - _AttackAmount; i++)
                {
                    Destroy(AttackDicePanel.transform.GetChild(i).gameObject);
                }
            }
        }

        if (_AttackDiceList.Count == 0)
            ResetDice();
        else
        {
            // display the list of dice rolls
            int index = 0;
            foreach (Text text in AttackDicePanel.GetComponentsInChildren<Text>())
            {
                text.text = _AttackDiceList[index].ToString();
                index++;
            }
        }
    }

    private void ResetDice()
    {
        // set all dice to 0. Tempory till we get pictures
        foreach (Text text in AttackDicePanel.GetComponentsInChildren<Text>())
        {
            text.text = AttackDice.Empty.ToString();
        }
    }

    protected override void SetDescription()
    {
        throw new NotImplementedException();
    }
}

