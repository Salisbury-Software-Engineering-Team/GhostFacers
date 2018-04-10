using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Path;
    [SerializeField]
    private GameObject GameManager;
    [SerializeField]
    private GameObject Piece;

    private void Start()
    {
        //Piece = this.transform.gameObject;
        //Debug.Log("Asigned" + Piece);
    }

    public void MovePiece(GameObject tile)
    {
        // if button is not blocked by player
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Piece = GameManager.GetComponent<GameState>().CurrentPiece;
            //Testing
            //TODO: Animate**********
            Piece.transform.position = tile.transform.position;
            Piece.GetComponent<CharacterPiece>().ClearHighlights(); // clear button highlights
        }
    }

    private void FindPath()
    {
        //List<GameObject> allPaths = Piece.GetComponent<CharacterPiece>().All
    }

}
