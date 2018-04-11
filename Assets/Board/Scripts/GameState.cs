using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private GameObject _currentPiece;
    public GameObject CurrentPiece
    {
        get { return _currentPiece;}
        set
        {
            if (value != _currentPiece && _currentPiece != null) // if current selected piece changed
            {
                _currentPiece.GetComponent<CharacterPiece>().ClearHighlights(); // clear highlights from old piece
                _currentPiece = value;
            }
            else
                _currentPiece = value;
        }
    }
    public int TotalMovement; //testing for movement

    [SerializeField]
    private List<Player> Players;

	void Start()
    {
        _currentPiece = CurrentPiece;
	}
}
