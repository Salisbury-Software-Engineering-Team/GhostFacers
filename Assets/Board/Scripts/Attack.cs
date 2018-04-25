using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<CharacterPiece> AttackablePieces; // Listof attaackable piece 
    private CharacterPiece _PieceAttacking;
    public CharacterPiece PieceToAttack;
    public bool doneAttack;
    public GameObject BtnAttackUI;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BtnAttackUI.SetActive(false);
    }

    public void BeginAttack(CharacterPiece pieceAttacking)
    {
        BtnAttackUI.SetActive(true);
        doneAttack = false;
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

    public void AttackPiece()
    {
        Debug.Log(PieceToAttack);
        Debug.Log(AttackablePieces);
        // Enemy Selected
        if (PieceToAttack != null)
        {
            BtnAttackUI.SetActive(false); // turn off attack button 
            //TODO: *************Finish attacking******************
            Debug.Log("Attacking Piece : " + PieceToAttack);
            EndAttack();
        }
        else
        {
            Debug.Log("No Piece to Attack Selected: ");
        }

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
        BtnAttackUI.SetActive(false); // turn off attack button 
        AttackablePieces.Clear();
        if (PieceToAttack != null)
            PieceToAttack.DisplaySelected(false); // unselected the attack piece
        _PieceAttacking = null;
        PieceToAttack = null;
        doneAttack = true;
    }

}
