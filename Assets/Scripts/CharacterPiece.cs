using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPiece : MonoBehaviour
{
    [SerializeField]
    private List<MovementTiles> AvaliableMovementTiles = new List<MovementTiles>();

    [SerializeField]
    private GameObject GameManager;

    [Serializable]
    private struct MovementTiles
    {
        public int move; // current tiles movement
        public GameObject tile; // tile object
        public MovementTiles(int m, GameObject t) { move = m; tile = t; }
    }

    [SerializeField]
    private GameObject Tile;

    [SerializeField]
    private bool isMoveShowing = false; // for if the movement tiles are showing highlighted

    /*
     * Action to take when the user click on the piece
     *
    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO:
        // Determine if piece belongs to current user.
        // Show avaible movement.
        Debug.Log("Testing clicked");
    }*/

    private void OnMouseUp()
    {
        Debug.Log("Character Clicked");
        //TODO: ADD check if it is current users turn.
        DisplayAvaliableMovement(GameManager.GetComponent<GameState>().TotalMovement);

        //TEsts*********************
        GameManager.GetComponent<GameState>().CurrentPiece = transform.gameObject;
    }

    /*
     * Display the total movement tiles
     */
    private void DisplayAvaliableMovement(int move)
    {
        ShowHighlights(false);
        AvaliableMovementTiles.Clear(); //make empty list first
        isMoveShowing = true; // set visiable to true
        GameObject currentTile = GetCurrentTile();
        GetAllAvaliableMovement(currentTile, move);
        ShowHighlights(true);
        currentTile.GetComponent<Tile>().Button.enabled = false; // hide current tiles button
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

    /*
     * Support Function for geting total movements
     * 
     * Still needs improvements !!!!
     */
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
                    MovementRecursive(currentMove + 1, totalMove, tile);
                }
            }
        }
    }

    /*
    * Show of hide all the highlighted tiles in the list of Avaiable tiles
    */
    private void ShowHighlights(bool doShow)
    {
        foreach (MovementTiles tile in AvaliableMovementTiles)
        {
            tile.tile.GetComponent<Tile>().Highlight.SetActive(doShow);
            tile.tile.GetComponent<Tile>().Button.enabled = doShow;
        }
    }

    public void ClearHighlights()
    {
        ShowHighlights(false);
        isMoveShowing = false;
    }
}
