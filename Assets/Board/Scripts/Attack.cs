using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    private enum AttackDice
    {
        Empty = 0,
        Miss_1 = 1,
        Miss_2= 2,
        Hit_1 = 3,
        Hit_2 = 4,
        Human = 5,
        Monster = 6,
    }

    public List<CharacterPiece> AttackablePieces; // Listof attaackable piece 
    private CharacterPiece _PieceAttacking;
    public CharacterPiece PieceToAttack;

    private bool _doneAttack;
    public bool DoneAttack { get { return _doneAttack; } }


    [SerializeField] private GameObject BtnAttackUI;
    [SerializeField] private GameObject AttackDiceUI;
    [SerializeField] private GameObject AttackDicePanel;
    [SerializeField] private GameObject AttackDicePrefab;
    [SerializeField] private Text TotalDamgeText;
    [SerializeField] private GameObject CannotAttackMessageUI;
    [SerializeField] private Text _AttackPieceText;
    [SerializeField] private Text _HelpText;

    private bool _DoneAttackRoll;
    [SerializeField] private bool _AppliedAttack;

    [SerializeField] private List<AttackDice> _AttackDiceList;
    private int _AttackAmount;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        BtnAttackUI.SetActive(false);
        AttackDiceUI.SetActive(false);
        _DoneAttackRoll = false;
        _AppliedAttack = false;
        _AttackAmount = 0;
        TotalDamgeText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (AttackDiceUI.activeInHierarchy)
            DisplayAttackDice();
        if (_DoneAttackRoll && !_AppliedAttack)
            ApplyAttack();
        if (_AttackPieceText && PieceToAttack)
            _AttackPieceText.text = "Attacking: " + PieceToAttack.Stat.Name;
    }

    public void SetupAttack(CharacterPiece pieceAttacking)
    {
        BtnAttackUI.SetActive(true);
        _doneAttack = false;
        TotalDamgeText.text = "Total Damge: 0";
        _PieceAttacking = pieceAttacking;
        Debug.Log("Attacking ");
        DeteremineAttackablePieces();
    }

    private void DeteremineAttackablePieces()
    {
        Tile currentTile = _PieceAttacking.GetComponent<CharacterPiece>().CurrentTile;
        List<GameObject> adjTiles = currentTile.GetAdjTiles();
        float radius = _PieceAttacking.transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4; // 1/4 the width of the model collider
        Vector3 pos;
        Collider[] hitColliders;

        // check each adjtile to see if piece occupies the tile
        foreach (GameObject adj in adjTiles)
        {
            pos = adj.transform.position;
            pos.y += _PieceAttacking.transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2; // half the hieght of the model collider

            hitColliders = Physics.OverlapSphere(pos, radius);

            // tile is blocked
            if (hitColliders.Length > 0)
            {
                // if piece on on other side then add to attackable
                if (hitColliders[0].transform.parent.GetComponent<CharacterPiece>().Stat.Side != GameManager.instance.CurrentSide)
                    AttackablePieces.Add(hitColliders[0].transform.parent.GetComponent<CharacterPiece>());
            }
        }

        // no pieces to attack
        if (AttackablePieces.Count == 0)
            EndAttack();
    }

    public void BeginAttack()
    {
        // Enemy Selected
        if (PieceToAttack != null)
        {
            if (canAttackPiece()) //piece is attackable
            {
                GameManager.instance.CanSelectePiece = false;
                BtnAttackUI.SetActive(false); // turn off attack button
                AttackDiceUI.SetActive(true); // turn on dice roll ui
            }
            else // can not attack piece, display message why.
            {
                CannotAttackMessageUI.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("No Piece Selected");
        }
    }

    /// <summary>
    /// Determine if the piece to attack is attackable by the current piece.
    /// 
    /// ex: Death cannot be attacked unless the piece holds a certain card.
    ///     This card will add Death to the atttackable piece types when the card is being held.
    /// </summary>
    /// <returns>True if piece can be attacked.</returns>
    private bool canAttackPiece()
    {
        // check to see if piece can be attacked by type.
        foreach (PieceType type in _PieceAttacking.Stat.AttackablePieces)
        {
            if (type == PieceToAttack.Stat.Type)
                return true; // piece is attackable type
        }
        return false; // can not attack piece
    }

    public void DontAttackPiece()
    {
        EndAttack();
    }

    /// <summary>
    /// This handles the end of the attack phase turn by reseting variables.
    /// </summary>
    private void EndAttack()
    {
        GameManager.instance.CanSelectePiece = true;
        BtnAttackUI.SetActive(false); // turn off attack button 
        AttackDiceUI.SetActive(false); // turn off dice
        AttackablePieces.Clear();
        if (PieceToAttack != null)
            PieceToAttack.DisplaySelected(false); // unselected the attack piece
        _PieceAttacking = null;
        PieceToAttack = null;
        _DoneAttackRoll = false;
        _AttackDiceList.Clear();
        _AttackAmount = 0;
        _AppliedAttack = false;

        _doneAttack = true;
    }

    private void DisplayAttackDice()
    {
        if (_HelpText) // display help message to roll dice
            _HelpText.text = "Roll to Attack";
        int currentAmountOfDice = AttackDicePanel.transform.childCount;
        _AttackAmount = _PieceAttacking.Stat.Attack;

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

    public bool SelectPieceToAttack(CharacterPiece piece)
    {
        if (AttackablePieces.Contains(piece)) // piece is attackable
        {
            PieceToAttack = piece;
            return true;
        }
        else // piece not attackable
        {
            if (PieceToAttack) // remove piece to attack becasue user selected different piece that could not be selected
            {
                PieceToAttack = null;
            }
            return false;
        }
    }

    //TODO: Handle increasing attack dice when playing effect cards.
    public void AddAttackDice()
    {

    }

    public void AttackDone()
    {
        EndAttack();
    }

    private void ApplyAttack()
    {
        int amountOfDamageDone = 0;
        foreach (AttackDice dice in _AttackDiceList)
        {
            if (dice == AttackDice.Hit_1 || dice == AttackDice.Hit_2) // hit applied for humans and monsters
            {
                amountOfDamageDone++;
            }
            else if (dice == AttackDice.Human && (_PieceAttacking.Stat.Type == PieceType.Human || _PieceAttacking.Stat.Type == PieceType.Angel))
            {
                // rolled human and piece attacking is human
                amountOfDamageDone++;
            }
            else if (dice == AttackDice.Monster && _PieceAttacking.Stat.Type != PieceType.Human && _PieceAttacking.Stat.Type != PieceType.Angel)
            {
                //rolled dice is monster and piece attacking is monster, then hit piece
                amountOfDamageDone++;
            }
        }
        PieceToAttack.DecreaseHealth(amountOfDamageDone); // apply damge
        _AppliedAttack = true;
        TotalDamgeText.text = "Total Damge: " +amountOfDamageDone;
    }
}
