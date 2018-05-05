using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[Serializable]
public class CharacterPiece : MonoBehaviour
{
    [SerializeField] private GameObject CharacterVisPlaceholder; 
    [SerializeField] private GameObject Tile; // prefab for tiles

    private List<GameObject> AvaliableMovementTiles; // list of total movement avaliable
    private List<GameObject> BlockedMovementTiles; // tiles with player on them
    private Tile _currentTile; // current tile piece is at
    public Tile CurrentTile
    {
        get { return _currentTile; }
    }

    public CharacterStat Stat; // stats for the piece
    public NavMeshAgent Agent; // For character movement
    public GameObject CharacterModel; // For character model
    public GameObject CharacterPlaceHolder; // For character model transfomation template
    public bool canMove; // piece can still roll if true
    public bool doneMove; // once character is done moving;

    private void Awake()
    {
        Init();
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
            //***********************TESTIng *******************************Remove Later***********************
            for (int i = 0; i < CharacterModel.transform.childCount; i ++)
                CharacterModel.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Stat.PieceColor;
        }
    }

    /// <summary>
    /// Set all vars.
    /// </summary>
    private void Init()
    {
        canMove = false;
        Agent = GetComponent<NavMeshAgent>();
        BlockedMovementTiles = new List<GameObject>();
        AvaliableMovementTiles = new List<GameObject>();
        doneMove = false;
    }


    private void OnMouseUp()
    {
        GameManager.instance.CurrentPiece = this;
    }

    /*
     * Display the total movement tiles
     */
    public void DisplayAvaliableMovement(int move)
    {
        if (_currentTile == null) { _currentTile = GetCurrentTile(); } // if Current tile is not set
        GetMovement(_currentTile.gameObject, move);
    }

    /*
     * Find the current tile object that the piece is on.
     */
    private Tile GetCurrentTile()
    {
        float radius = transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider item in hitColliders)
        {
            if (item.gameObject.GetType() == Tile.GetType())
            {
                //Debug.Log("Found Tile");
                //Debug.Log(item.gameObject.transform.parent.gameObject);
                return item.gameObject.transform.parent.gameObject.GetComponent<Tile>();
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
                        tempPos.y += transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2; // half the hieght of the piece collider
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
        foreach (GameObject tile in AvaliableMovementTiles)
        {
            tile.GetComponent<Tile>().HighlightTile(false);
        }
        AvaliableMovementTiles.Clear();
    }

    public void SetCurrentTile(GameObject tile)
    {
        _currentTile = tile.GetComponent<Tile>();
    }

    public void DisplaySelected(bool isSelected)
    {
        transform.Find("Selected").gameObject.SetActive(isSelected);
    }

    /// <summary>
    /// Call this once the character piece has finished its turn to reset any vars that 
    /// need to be used again
    /// </summary>
    public void EndTurn()
    {
        doneMove = false; // reset done move so it can be used in anouth turn
        DisplaySelected(false); // remove highlight
    }

    /// <summary>
    /// Call this at the begining of the player turn to make sure all vars are propely set for each
    /// players piece.
    /// </summary>
    public void SetupTurn()
    {
        doneMove = false;
        canMove = true;
    }

    /// <summary>
    /// Decrease the characters health by the passed amount. Also Handle any damge reducing abilities here.
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseHealth(int amount)
    {
        if (Stat.Health-amount > 0)
        {
            Stat.Health = Stat.Health - amount;
        }
        else
        {
            Stat.Health = 0;
        }
    }
}
