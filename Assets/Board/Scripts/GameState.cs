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
            Debug.Log("Setting _currentPiece");
            if (value != _currentPiece)
            {
                _currentPiece.GetComponent<CharacterPiece>().ClearHighlights();
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
        foreach (var item in Players)
        {
            Debug.Log(item);

        }
	}
}
