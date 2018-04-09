using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPiece : MonoBehaviour
{


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
    }
}
