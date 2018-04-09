using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<GameObject> AdjTiles; // array of adj tiles 
    private Vector3 CurrentPosition; // current position of the tile
    private Vector3 size; // size of tile
    private float radius = 10;
    private string strTileModelName = "Tile_Model";
    private string strWallName = "Wall_Model";

    private void Awake()
    {
    }

    private void Start()
    {
        CurrentPosition = transform.position;
        size = transform.Find(strTileModelName).GetComponent<Renderer>().bounds.size;
        FindNeighbors();
    }

    public void TestClicked()
    {
        Debug.Log("Clicked Tile");
    }

    private void OnValidate()
    {
    }

    /*
     * Find adj Tiles
     */
    private void FindNeighbors()
    {

        float zOffset = size.z; // offset
        float xOffset = size.x;

        Vector3 CurrentPositionNorthOffset = CurrentPosition;
        Vector3 CurrentPositionSouthOffset = CurrentPosition;
        Vector3 CurrentPositionEastOffset = CurrentPosition;
        Vector3 CurrentPositionWestOffset = CurrentPosition;

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
        for (int i = 0; i < hitCollidersNorth.Length; i++)
        {
            if (hitCollidersNorth[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }
            if (hitCollidersNorth[i].gameObject.name == strTileModelName 
                && hitCollidersNorth[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersNorth[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null)
            AdjTiles.Add(temp);

        wallFound = false;
        for (int i = 0; i < hitCollidersSouth.Length; i++)
        {
            if (hitCollidersSouth[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }
            if (hitCollidersSouth[i].gameObject.name == strTileModelName 
                && hitCollidersSouth[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersSouth[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null)
            AdjTiles.Add(temp);

        wallFound = false;

        for (int i = 0; i < hitCollidersEast.Length; i++)
        {
            if (hitCollidersEast[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }
            if (hitCollidersEast[i].gameObject.name == strTileModelName 
                && hitCollidersEast[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersEast[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null)
            AdjTiles.Add(temp);

        wallFound = false;

        for (int i = 0; i < hitCollidersWest.Length; i++)
        {
            if (hitCollidersWest[i].gameObject.name == strWallName)
            {
                wallFound = true;
                temp = null;
                break;
            }
            if (hitCollidersWest[i].gameObject.name == strTileModelName 
                && hitCollidersWest[i].gameObject.transform.parent.gameObject != transform.gameObject)
                temp = hitCollidersWest[i].gameObject.transform.parent.gameObject;
        }
        if (!wallFound && temp != null)
            AdjTiles.Add(temp);

        Debug.Log("Done Finding Neighbors");
    }
}
