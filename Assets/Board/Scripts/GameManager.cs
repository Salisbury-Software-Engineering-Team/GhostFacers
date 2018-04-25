using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // GameManager object for players to access
    private Attack _Attack;
    private TurnManager _turn; // manages a character turn once they hit roll.
    public TurnManager Turn
    {
        get { return _turn; }
    }

    [SerializeField] private CharacterPiece _currentPiece;
    public CharacterPiece CurrentPiece
    {
        get { return _currentPiece; }
        set
        {
            if (value != _currentPiece)
                DetermineSelecetion(value);
        }
    }

    public int TotalMovement; //testing for movement
    public SideType CurrentSide; // Current sides turn

    //Testing ************** Delete When Done
    public Text PhaseText;
    public Text CurrentPlayerText;
    public Text CurrentPieceText;

    [SerializeField] private List<Player> GoodPlayers;
    [SerializeField] private List<Player> EvilPlayers;
    [SerializeField] private Player _currentPlayer;
    public Player CurrentPlayer
    {
        get { return _currentPlayer; }
    }
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

        StartCoroutine(StartGame());
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
        _Attack = GetComponent<Attack>();
    }

    private IEnumerator StartGame()
    {
        //TODO: Setup

        while (WinningSide == -1)
        {
            Debug.Log("Good Turn");
            yield return GoodPlayersTurn();
            Debug.Log("Evil Turn");
            yield return EvilPlayersTurn();
        }
    }

    /// <summary>
    /// Sets the Current selected piece only if it belongs to the current players turn. Also Displays any 
    /// info about  the current piece. CuurentPiece will be used to determine if charater can roll.
    ///         
    /// </summary>
    /// <param name="piece">Piece to display info about when selected</param>
    /// <returns>Character piece if can roll, else null</returns>
    private void DetermineSelecetion(CharacterPiece piece)
    {
        Debug.Log("Selected Piece " + piece + " Turn Phase: " + _turn.TurnPhase);
        if (_turnStarted)
        {
            // Hnadle differently not sure how yet......
            if (_turn.TurnPhase == Phase.Attack)
            {
                // piece is attackable
                if (_Attack.AttackablePieces.Contains(piece))
                {
                    Debug.Log("Piece Selcted is attackable");
                    if (_Attack.PieceToAttack != null)
                        _Attack.PieceToAttack.DisplaySelected(false);
                    piece.DisplaySelected(true);
                    _currentPiece = piece;
                    _Attack.PieceToAttack = piece;
                }
            }
        }
        else
        {
            SetCurrentPiece(piece);
        }
    }

    /// <summary>
    /// Sets the Current selected piece only if it belongs to the current players turn. Also Displays any 
    /// info about  the current piece. CuurentPiece will be used to determine if charater can roll.
    ///         
    /// </summary>
    /// <param name="piece">Piece to display info about when selected</param>
    /// <returns>Character piece if can roll, else null</returns>
    private void SetCurrentPiece(CharacterPiece piece)
    {
        if (_currentPiece != null)
        {
            _currentPiece.DisplaySelected(false);
            RollButton.enabled = false;
        }
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
                    _currentPiece = piece;
                }
                else
                    _currentPiece = null;
            }
            else // Piece belongs to friend info
            {
                // TODO: Display character info about the piece as friend
                _currentPiece = null;
            }
        }
        else // Piece beongs to enemy
        {
            //TODO: Display emey piece info
            _currentPiece = null;
        }
    }

    private IEnumerator GoodPlayersTurn()
    {
        CurrentSide = SideType.Good;
        foreach (Player play in GoodPlayers)
        {
            _currentPlayer = play;
            play.SetUpTurn();
            yield return new WaitUntil(() => play.TotalPiecesLeftToMove == 0);
        }
        //yield return new WaitUntil(() => _Attack.doneAttack == true); // wait till all good have done turn
    }

    private IEnumerator EvilPlayersTurn()
    {
        CurrentSide = SideType.Evil;
        foreach (Player play in EvilPlayers)
        {
            _currentPlayer = play;
            play.SetUpTurn();
            yield return new WaitUntil(() => play.TotalPiecesLeftToMove == 0);
        }
    }


}
