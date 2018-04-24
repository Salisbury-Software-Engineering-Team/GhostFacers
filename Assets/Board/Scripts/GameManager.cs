using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // GameManager object for players to access

    [SerializeField] private CharacterPiece _currentPiece;
    public CharacterPiece CurrentPiece
    {
        get { return _currentPiece; }
        set
        {
            _currentPiece = SetCharacterPieceTurn(value);
        }
    }

    public int TotalMovement; //testing for movement
    public SideType CurrentSide; // Current sides turn
    private TurnManager _turn; // manages a character turn once they hit roll.
    public TurnManager Turn
    {
        get { return _turn; }
    }

    //Testing ************** Delete When Done
    public Text PhaseText;
    public Text CurrentPlayerText;
    public Text CurrentPieceText;

    [SerializeField] private List<Player> GoodPlayers;
    [SerializeField] private List<Player> EvilPlayers;
    [SerializeField] private Player CurrentPlayer;
    [SerializeField] private Button RollButton;
    [SerializeField] private bool _turnStarted;
    public bool TurnStarted
    {
        get { return _turnStarted; }
        set
        {
            // Turn started changed
            if (value != _turnStarted)
            {
                //TEsing ***************************** Change later
                RollButton.enabled = !value;
                _turnStarted = value;
            }
        }
    }

    private int WinningSide; // winning side, compare to sideType enum to get a result. -1 = no winner

	private void Start()
    {
        Init();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        _currentPiece = CurrentPiece;

        // TEsting *********************
        GoodPlayersTurn();
	}

    private void Update()
    {
        //Testing ********
        if (_currentPiece != null)
            CurrentPieceText.text = "Current Piece " + _currentPiece.gameObject.ToString();
        CurrentPlayerText.text = "Current Player " + CurrentPlayer.gameObject.ToString();
    }

    private void Init()
    {
        WinningSide = -1;
        TurnStarted = false;
        _turn = this.GetComponent<TurnManager>();
    }

    private void StartGame()
    {
        //TODO: Setup

            GoodPlayersTurn();
        while (WinningSide == -1)
        {
            //EvilPlayersTurn();
        }
    }

    /// <summary>
    /// Sets the Current selected piece only if it belongs to the current players turn. Also Displays any 
    /// info about  the current piece. CuurentPiece will be used to determine if charater can roll.
    ///         
    /// </summary>
    /// <param name="piece">Piece to display info about when selected</param>
    /// <returns>Character piece if can roll, else null</returns>
    private CharacterPiece SetCharacterPieceTurn(CharacterPiece piece)
    {
        // hide current pieces highlights
        // ****************Might be able to remove this ************************
        if (_currentPiece != null)
        {
            //_currentPiece.ClearHighlights();
            _currentPiece.DisplaySelected(false);
            RollButton.enabled = false;
        }
        //************************************************************************

        // Means the piece belongs to the current sides turn
        if (CurrentSide == piece.Stat.Side)
        {
            // Piece belongs to Current Player
            if (CurrentPlayer != null && CurrentPlayer.Pieces != null && CurrentPlayer.Pieces.Contains(piece))
            {
                // TODO: Display Current Players piece info **********************

                if (piece.canMove && !_turnStarted) // piece can still roll.
                {
                    RollButton.enabled = true;
                    piece.DisplaySelected(true);

                    return piece;
                }
                else
                    return null;
            }
            else // Piece belongs to friend info
            {
                // TODO: Display character info about the piece as friend
                return null;
            }
        }
        else // Piece beongs to enemy
        {
            //TODO: Display emey piece info
            return null;
        }
    }

    private void GoodPlayersTurn()
    {
        CurrentSide = SideType.Good;
        foreach (Player play in GoodPlayers)
        {
            foreach (CharacterPiece piece in play.Pieces)
            {
                piece.SetupTurn();
                Debug.Log(play + " Piece: " + piece.canMove);
            }
        }
    }

    private void EvilPlayersTurn()
    {
        CurrentSide = SideType.Evil;
    }


}
