using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Piece = GameManager.GetComponent<GameState>().CurrentPiece;
        Debug.Log(Piece);
        Debug.Log("Clicked Tile");
        //Testing
        Debug.Log(Piece.transform.gameObject);
        Piece.transform.position = tile.transform.position;
        Debug.Log(Piece.transform.position);
        Piece.GetComponent<CharacterPiece>().ClearHighlights();
    }

    private void FindPath()
    {
        //List<GameObject> allPaths = Piece.GetComponent<CharacterPiece>().All
    }

}
