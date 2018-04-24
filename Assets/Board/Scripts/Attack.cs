using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<CharacterPiece> AttackablePieces; // Listof attaackable piece 
    public bool doneAttack = false;

    public void BeginAttack()
    {
        DeteremineAttackablePieces();
    }

    private void DeteremineAttackablePieces()
    {
        Tile currentTile = GetComponent<CharacterPiece>().CurrentTile;
        List<GameObject> adjTiles = currentTile.GetAdjTiles();
        float radius = transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4; // 1/4 the width of the model collider
        Vector3 pos;
        Collider[] hitColliders;

        foreach (GameObject adj in adjTiles)
        {
            pos = adj.transform.position;
            pos.y += transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2; // half the hieght of the model collider

            hitColliders = Physics.OverlapSphere(pos, radius);

            // tile is blocked
            if (hitColliders.Length > 0)
            {
                AttackablePieces.Add(adj.GetComponent<CharacterPiece>());
            }
        }
    }

}
