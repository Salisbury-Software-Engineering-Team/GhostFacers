using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // GameManager object for players to access
    private Attack _Attack; // Attack script
    private TurnManager _turn; // manages a character turn once they hit roll.
    public TurnManager Turn
    {
        get { return _turn; }
    }

    public bool CanSelectePiece; // Determine if piece can be selected
    [SerializeField] private CharacterPiece _currentPiece;
    public CharacterPiece CurrentPiece
    {
        get { return _currentPiece; }
        set
        {
            if (CanSelectePiece)
                DetermineSelecetion(value);
        }
    }

    public int TotalMovement; //testing for movement
    public SideType CurrentSide; // Current sides turn

    [SerializeField] private List<Player> GoodPlayers; // lIst of all good players
    [SerializeField] private List<Player> EvilPlayers; // list of all Evil players
    [SerializeField] private Player _currentPlayer; // current player
    public Player CurrentPlayer
    {
        get { return _currentPlayer; }
    }

    private bool _turnStarted;
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

    [SerializeField] private SideType WinningSide; // winning side, compare to sideType enum to get a result. -1 = no winner

    [SerializeField] private Button RollButton;
    [SerializeField] private Text _CurrentSideText;

	private void Awake()
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
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (_CurrentSideText)
            _CurrentSideText.text = "Turn: " + CurrentSide;
        if (CheckForWinner())
            WinnerFound();

    }

    private void Init()
    {
        WinningSide = SideType.None;
        TurnStarted = false;
        _turn = this.GetComponent<TurnManager>();
        _Attack = GetComponent<Attack>();
        CanSelectePiece = true;
        RollButton.gameObject.SetActive(false);
    }

    private IEnumerator StartGame()
    {
        SetupPiecesForNewGame(); // call setup
        while (WinningSide == SideType.None) // loop till a side has no pieces left
        {
            if (CurrentSide == SideType.Good) // good turn
                yield return GoodPlayersTurn();
            else // evils turn
                yield return EvilPlayersTurn();
        }
    }

    private void SetupPiecesForNewGame()
    {
        foreach (Player play in GoodPlayers)
        {
            foreach (CharacterPiece piece in play.Pieces)
            {
                if (piece)
                    piece.Stat.SetupStats();
            }
        }

        foreach (Player play in EvilPlayers)
        {
            foreach (CharacterPiece piece in play.Pieces)
            {
                piece.Stat.SetupStats();
            }
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
        //Debug.Log("Selected Piece " + piece + " Turn Phase: " + _turn.TurnPhase);
        if (_turnStarted)
        {
            // Hnadle differently not sure how yet......
            if (_turn.TurnPhase == Phase.Attack)
            {
                // piece is attackable
                if (_Attack.SelectPieceToAttack(piece))
                {
                    if (_Attack.PieceToAttack != null)
                        _Attack.PieceToAttack.DisplaySelected(false);
                    piece.DisplaySelected(true); // highlight selecteced piece
                }
            }
        }
        SetCurrentPiece(piece); // select piece

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
        if (_currentPiece != null && _currentPiece != Turn.Piece)
        {
            _currentPiece.DisplaySelected(false);
        }

        RollButton.gameObject.SetActive(false);
        RollButton.enabled = false;

        // Means the piece belongs to the current sides turn
        if (CurrentSide == piece.Stat.Side)
        {
            // Piece belongs to Current Player
            if (CurrentPlayer != null && CurrentPlayer.Pieces != null && CurrentPlayer.Pieces.Contains(piece))
            {
                // TODO: Display Current Players piece info **********************

                if (piece.canMove && !_turnStarted) // piece can still roll.
                {
                    RollButton.gameObject.SetActive(true);
                    RollButton.enabled = true;
                    piece.DisplaySelected(true);
                    _currentPiece = piece;
                }
            }
        }
        _currentPiece = piece;
    }

    private IEnumerator GoodPlayersTurn()
    {
        foreach (Player play in GoodPlayers)
        {
            _currentPlayer = play;
            play.SetUpTurn();
            yield return new WaitUntil(() => play.TotalPiecesLeftToMove == 0);
        }
        CurrentSide = SideType.Evil; // it is now evils turn
    }

    private IEnumerator EvilPlayersTurn()
    {;
        foreach (Player play in EvilPlayers)
        {
            _currentPlayer = play;
            play.SetUpTurn();
            yield return new WaitUntil(() => play.TotalPiecesLeftToMove == 0);
        }
        CurrentSide = SideType.Good; // set current side to goods turn
    }

    private bool CheckForWinner()
    {
        int goodDead = 0;
        int evilDead = 0;
        foreach (Player play in GoodPlayers)
        {
            if (play.TotalPieceCount == 0)
                goodDead++;
        }

        if (goodDead == GoodPlayers.Count)
        {
            WinningSide = SideType.Evil;
            return true;
        }

        foreach (Player play in EvilPlayers)
        {
            if (play.TotalPieceCount == 0)
                evilDead++;
        }

        if (evilDead == EvilPlayers.Count)
        {
            WinningSide = SideType.Good;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Hadles what to do when a winner is found in the game.
    /// </summary>
    private void WinnerFound()
    {

    }


}
