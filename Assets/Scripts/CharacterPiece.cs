using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPiece : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> AvaliableMovementTiles;

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
        GetCurrentTile();
    }

    private void DisplayAvaliableMovement()
    {

    }

    private GameObject GetCurrentTile()
    {
        float radius = transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider item in hitColliders)
        {
            if (item.gameObject.name == "Tile_Model")
            {
                Debug.Log("Found Tile");
                Debug.Log(item.gameObject.transform.parent.gameObject);
                return item.gameObject.transform.parent.gameObject;
            }
        }

        // no tile found
        return null;
    }
}
