using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    [SerializeField] private CharacterPiece Piece; // Piece to move

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
            Piece = GameManager.instance.Turn.Piece;
            GameManager.instance.Turn.BtnDontMove.SetActive(false);
            //TODO: Animate**********
            GameManager.instance.CurrentPiece = Piece;
            Piece.Agent.SetDestination(tile.transform.position);
            Piece.SetCurrentTile(tile);
            Piece.StartCoroutine(WaitForAgent());
        }
    }

    /// <summary>
    /// Waits for the character piece to finish moving and then sets the character piece to doneMoving.
    /// This is so the player has to wait for the animation to complete before moving to the draw card phase.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForAgent()
    {
        yield return new WaitUntil(() => pathComplete() == true); // wait for the character to reach the selected tile

        Piece.ClearHighlights(); // clear button highlights
        Piece.doneMove = true;
    }

    private bool pathComplete()
    {
        if (Vector3.Distance(Piece.Agent.destination, Piece.Agent.transform.position) <= Piece.Agent.stoppingDistance)
        {
            if (!Piece.Agent.hasPath || Piece.Agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }
}
