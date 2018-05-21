using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tile : MonoBehaviour
{
    public Button Button; 
    public GameObject Portal; // link to portal

    [SerializeField] private List<GameObject> AdjTiles; // array of adj tiles 
    [SerializeField] private GameObject Highlight; // hightlight sprite
    [SerializeField] public TileType Type; // type of tile
    //public TileType Type { get { return _type; } }
    [SerializeField] private StartingZone Zone;
    private Vector3 CurrentPosition; // current position of the tile
    private Vector3 size; // size of tile

    // Used for finding objects to the north, wouth, east and west of current object. 
    private float radius; // size of collider check
    private string strTileModelName = "Tile_Model"; // name eof tile model prefab
    private string strWallName = "Wall_Model"; // name of wall model prefab
    private string strButtonName = "Tile_Button"; // name of button prefab

    private void Start()
    {
        CurrentPosition = transform.position; // set current pos.
        size = transform.Find(strTileModelName).GetComponent<Renderer>().bounds.size; // get current tiles model size
        radius = size.x / 4;
        //Debug.Log(radius);
        FindNeighbors();

        // add portal as adj tile
        if (Portal != null)
        {
            AdjTiles.Add(Portal);
        }
        if (Zone != StartingZone.None)
            GameManager.instance.DisplayStartingZone += (Zone, doHighlight) => DisplayStartingZone(Zone, doHighlight); // displays tarting zone when called
    }

    /*
     * Called on inspector update
     */
    private void OnValidate()
    {
        RefreshUI();
    }

    /*
     * Find adj Tiles
     */
    private void FindNeighbors()
    {

        float zOffset = size.z; // offset
        float xOffset = size.x;

        // set all adj tiles to current tile pos
        Vector3 CurrentPositionNorthOffset = CurrentPosition;
        Vector3 CurrentPositionSouthOffset = CurrentPosition;
        Vector3 CurrentPositionEastOffset = CurrentPosition;
        Vector3 CurrentPositionWestOffset = CurrentPosition;

        // adj adj tile check
        // check will be placed at edge of current tile with a radius of the radius var.
        CurrentPositionNorthOffset.z += zOffset/2;
        CurrentPositionSouthOffset.z -= zOffset/2;
        CurrentPositionEastOffset.x += xOffset/2;
        CurrentPositionWestOffset.x -= +xOffset/2;

        //Find all objects that colide
        Collider[] hitCollidersNorth = Physics.OverlapSphere(CurrentPositionNorthOffset, radius);
        Collider[] hitCollidersSouth = Physics.OverlapSphere(CurrentPositionSouthOffset, radius);
        Collider[] hitCollidersEast = Physics.OverlapSphere(CurrentPositionEastOffset, radius);
        Collider[] hitCollidersWest = Physics.OverlapSphere(CurrentPositionWestOffset, radius);

        // Check the north, south, east and west tiles to see if there is a connected tile.
        // If a wall is found, then end check and move to next loop.
        //
        // Also does not add the current tile to teh list. 
        GameObject temp = null;
        bool wallFound = false;

        // Add any tiles above
        for (int i = 0; i < hitCollidersNorth.Length; i++)
        {
            //Wall found, then end check for adj tiles.
            if (hitCollidersNorth[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }

            // If object is named strModelName and not current object, then set as temp.
            // Will only be added if no wall is found.
            if (hitCollidersNorth[i].gameObject.name == strTileModelName 
                && hitCollidersNorth[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersNorth[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null && !AdjTiles.Contains(temp))
            AdjTiles.Add(temp);

        wallFound = false;

        // Add any tiles below
        for (int i = 0; i < hitCollidersSouth.Length; i++)
        {
            //Wall found, then end check for adj tiles.
            if (hitCollidersSouth[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }

            // If object is named strModelName and not current object, then set as temp.
            // Will only be added if no wall is found.
            if (hitCollidersSouth[i].gameObject.name == strTileModelName 
                && hitCollidersSouth[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersSouth[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null && !AdjTiles.Contains(temp))
            AdjTiles.Add(temp);

        wallFound = false;

        // Add any tiles to the right
        for (int i = 0; i < hitCollidersEast.Length; i++)
        {
            //Wall found, then end check for adj tiles.
            if (hitCollidersEast[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }

            // If object is named strModelName and not current object, then set as temp.
            // Will only be added if no wall is found.
            if (hitCollidersEast[i].gameObject.name == strTileModelName 
                && hitCollidersEast[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersEast[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null && !AdjTiles.Contains(temp))
            AdjTiles.Add(temp);

        wallFound = false;

        //add any tiles to the left
        for (int i = 0; i < hitCollidersWest.Length; i++)
        {
            //Wall found, then end check for adj tiles.
            if (hitCollidersWest[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }

            // If object is named strModelName and not current object, then set as temp.
            // Will only be added if no wall is found.
            if (hitCollidersWest[i].gameObject.name == strTileModelName 
                && hitCollidersWest[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersWest[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null && !AdjTiles.Contains(temp))
            AdjTiles.Add(temp);

        //Debug.Log("Done Finding Neighbors");
    }

    /*
     * Use this to change the visable type of the Tile when type enum is changed.
     * Ex:
     *  Display weapon tile visual when tile is change to weapon.
     */
    private void RefreshUI()
    {
        Image image = transform.Find(strButtonName).GetComponent<Image>();
        // if type has image
        if (Type)
        {
            if (Type.TileImage == null)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
                image.sprite = Type.TileImage;
            }

            // Set color
            image.color = Type.ButtonColor;
        }
    }

    /*
     * Mkaes the Button clickable and turns on the Highlight sprite
     */
    public void HighlightTile(bool doShow)
    {
        Highlight.SetActive(doShow);
        Button.enabled = doShow;
    }

    public List<GameObject> GetAdjTiles() { return AdjTiles; }

    public void DisplayStartingZone(StartingZone z, bool doHighlight)
    {
        if (Zone == z)
        {
            HighlightTile(doHighlight);
        }
    }
}
