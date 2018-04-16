using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[Serializable]
public class CharacterPiece : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> AvaliableMovementTiles; // list of total movement avaliable
    private List<GameObject> BlockedMovementTiles; // tiles with player on them
    private GameObject CurrentTile; // current tile piece is at

    [SerializeField]
    private GameObject Tile;

    private bool isMoveShowing = false; // for if the movement tiles are showing highlighted

    public CharacterStat Stat; // stats for the piece

    public NavMeshAgent Agent;

    public GameObject CharacterModel;
    public GameObject CharacterPlaceHolder;
    [SerializeField]
    private GameObject CharacterVisPlaceholder;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        BlockedMovementTiles = new List<GameObject>();
        AvaliableMovementTiles = new List<GameObject>();
        Destroy(CharacterVisPlaceholder);
        if (Stat != null) // creates character model based off of the stat model chosen
        {
            if (CharacterModel == null)
            {
                GameObject thisModel = Instantiate(Stat.Model, CharacterPlaceHolder.transform.position, CharacterPlaceHolder.transform.rotation, CharacterPlaceHolder.transform) as GameObject;
                CharacterModel = thisModel;
            }
            else
            {
                Destroy(CharacterModel);
                CharacterModel = Instantiate(Stat.Model, CharacterPlaceHolder.transform.position, CharacterPlaceHolder.transform.rotation, CharacterPlaceHolder.transform) as GameObject;
            }
        }
    }

    private void OnValidate()
    {
    }

    private void OnMouseUp()
    {
        //TEsts*********************
        //Stat.Weapons.Add(new Card("Testing", "Boom"));
        GameManager.instance.CurrentPiece = transform.gameObject;
        //TODO: ADD check if it is current users turn.
        DisplayAvaliableMovement(GameManager.instance.TotalMovement);

    }

    /*
     * Display the total movement tiles
     */
    private void DisplayAvaliableMovement(int move)
    {
        isMoveShowing = true; // set visiable to true
        if (CurrentTile == null) { CurrentTile = GetCurrentTile(); } // if Current tile is not set
        GetMovement(CurrentTile, move);
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
 
    private void GetMovement(GameObject startTile, int totalMove)
    {
        Queue<GameObject> check = new Queue<GameObject>();
        check.Enqueue(startTile);

        // check if player is in tile location
        Vector3 tempPos;
        Collider[] hitColliders;

        AvaliableMovementTiles.Clear();
        AvaliableMovementTiles.Add(startTile);

        for (int k = 0; k < totalMove; k++)
        {
            Queue<GameObject> temp = new Queue<GameObject>();
            foreach (GameObject current in check)
            {
                // loop for each adj tile
                for (int i = 0; i < current.GetComponent<Tile>().GetAdjTiles().Count; i++)
                {
                    GameObject next = current.GetComponent<Tile>().GetAdjTiles()[i];
                    if (!AvaliableMovementTiles.Contains(next))
                    {
                        // Tile is portal, then skip current tile
                        if (next.GetComponent<Tile>().Portal != null)
                        {
                            temp.Enqueue(next); // enque current tile
                            AvaliableMovementTiles.Add(next); // add curent tile to movment list 
                            next = next.GetComponent<Tile>().Portal; // make current tile = portal destination, then also add that tile
                        }
                        else { next.GetComponent<Tile>().HighlightTile(true); } // not a portal tile

                        temp.Enqueue(next);
                        AvaliableMovementTiles.Add(next);

                        // look for another player piece
                        tempPos = next.transform.position;
                        tempPos.y += transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2;
                        hitColliders = Physics.OverlapSphere(tempPos, transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4);

                        // tile is blocked
                        if (hitColliders.Length > 0)
                        {
                            BlockedMovementTiles.Add(next);
                        }

                    }
                }
            }
            check = temp;
        }

        // hide blocked tiles. EX tiles with other character pieces on them.
        foreach (GameObject tile in BlockedMovementTiles)
        {
            tile.GetComponent<Tile>().HighlightTile(false);
        }

        //clear Blocked tiles list
        BlockedMovementTiles.Clear();

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

    public void SetCurrentTile(GameObject tile)
    {
        CurrentTile = tile;
    }
}
