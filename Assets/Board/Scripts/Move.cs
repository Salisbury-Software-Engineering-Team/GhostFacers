using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    [SerializeField] private CharacterPiece Piece; // Piece to move

    private void Start()
    {
        //Piece = this.transform.gameObject;
        //Debug.Log("Asigned" + Piece);
    }

    /// <summary>
    /// Nav Mesh is used to find a path to the selcted tile by using the agent var from
    /// the character piece.
    /// </summary>
    /// <param name="tile">Destination tile selected</param>
    public void MovePiece(GameObject tile)
    {
        // if button is not blocked by player
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Piece = GameManager.instance.CurrentPiece;

            //TODO: Animate**********
            Piece.canMove = false;
            Piece.Agent.SetDestination(tile.transform.position);
            Piece.ClearHighlights(); // clear button highlights
            Piece.SetCurrentTile(tile);
            Piece.doneMove = true;
        }
    }
}
