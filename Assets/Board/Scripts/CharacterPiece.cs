using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPiece : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> AvaliableMovementTiles; // list of total movement avaliable
    public GameObject CurrentTile; // current tile piece is at
    [SerializeField]
    private GameObject GameManager;

    [SerializeField]
    private GameObject Tile;

    [SerializeField]
    private bool isMoveShowing = false; // for if the movement tiles are showing highlighted

    [SerializeField]
    public CharacterStat Stat; // stats for the piece

    private void OnMouseUp()
    {
        //TEsts*********************
        Stat.Weapons.Add(new Card("Testing", "Boom"));
        GameManager.GetComponent<GameState>().CurrentPiece = transform.gameObject;
        //TODO: ADD check if it is current users turn.
        DisplayAvaliableMovement(GameManager.GetComponent<GameState>().TotalMovement);

    }

    /*
     * Display the total movement tiles
     */
    private void DisplayAvaliableMovement(int move)
    {
        isMoveShowing = true; // set visiable to true
        if (CurrentTile == null) { CurrentTile = GetCurrentTile(); } // if Current tile is not set
        GetAllAvaliableMovement(CurrentTile, move);
    }

    /*
     * Find the current tile object that the piece is on.
     */
    private GameObject GetCurrentTile()
    {
        float radius = transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider item in hitColliders)
        {
            if (item.gameObject.GetType() == Tile.GetType())
            {
                //Debug.Log("Found Tile");
                //Debug.Log(item.gameObject.transform.parent.gameObject);
                return item.gameObject.transform.parent.gameObject;
            }
        }

        // no tile found
        return null;
    }

    private void GetAllAvaliableMovement(GameObject currentTile, int totalMove)
    {
        int currentMovement = 1;
        MovementRecursive(currentMovement, totalMove, currentTile);
    }

    private void MovementRecursive(int move, int total, GameObject tile)
    {
        // Used to check if another piece occupies tile.
        Vector3 tempPos;
        Collider[] hitColliders;
        // check each adj tile
        foreach (GameObject adj in tile.GetComponent<Tile>().GetAdjTiles())
        {
            tempPos = adj.transform.position;
            tempPos.y += transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2;
            hitColliders = Physics.OverlapSphere(tempPos, transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4);

            // no piece at tile
            if (hitColliders.Length == 0)
            {
                // add to list if not already in list
                if (!AvaliableMovementTiles.Contains(adj))
                {
                    AvaliableMovementTiles.Add(adj);
                    adj.GetComponent<Tile>().HighlightTile(true);
                }
            }
        }

        move++;

        // recursive call if movement still left
        if (move <= total)
        {
            foreach (GameObject adj in tile.GetComponent<Tile>().GetAdjTiles())
            {
                MovementRecursive(move, total, adj);
            }
        }
    }


    /*
     * Hides Highlights and clears movement tile array.
     */
    public void ClearHighlights()
    {
        isMoveShowing = false;
        foreach (GameObject tile in AvaliableMovementTiles)
        {
            tile.GetComponent<Tile>().HighlightTile(false);
        }
        AvaliableMovementTiles.Clear();
    }
}

/********** Testiing a different way to get path might not be better, needs more testing
//private List<MovementTiles> AvaliableMovementTiles = new List<MovementTiles>();
[Serializable]
private struct MovementTiles
{
    public int move; // current tiles movement
    public GameObject tile; // tile object
    public MovementTiles(int m, GameObject t) { move = m; tile = t; }
}

 * Support Function for geting total movements
 * 
 * Still needs improvements !!!!
 *
private void MovementRecursive(int move, int totalMove, GameObject currentTile)
{
    bool found;
    List<GameObject> tiles = currentTile.GetComponent<Tile>().GetAdjTiles();
    int currentMove = move;
    foreach (GameObject tile in tiles)
    {
        found = false;
        if (move <= totalMove)
        {
            //Debug.Log("Move Rec of inside IF " + move + " " + currentTile);
            // current tiles move is smaller then tiles stored move
            MovementTiles t;
            for (int i = 0; i < AvaliableMovementTiles.Count; i++)
            {
                t = AvaliableMovementTiles[i];
                if (t.tile == tile) // tile already in list
                {
                    found = true;
                    if (currentMove < t.move) // tile found with smaller movement
                    {
                        t.move = currentMove;
                        MovementRecursive(currentMove + 1, totalMove, tile);
                    }
                    break;
                }
            }

            if (!found)
            {
                //tile not in list
                MovementTiles temp;
                temp.move = currentMove;
                temp.tile = tile;
                AvaliableMovementTiles.Add(temp);
                temp.tile.GetComponent<Tile>().HighlightTile(true);
                MovementRecursive(currentMove + 1, totalMove, tile);
            }
        }
    }
}*/
