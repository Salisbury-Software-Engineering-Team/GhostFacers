using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // GameManager object for players to access

    public bool CanSelectePiece; // Determine if piece can be selected
    public int TotalMovement; //testing for movement
    public SideType CurrentSide; // Current sides turn

    [SerializeField] private List<Player> _GoodPlayers; // lIst of all good players
    [SerializeField] private List<Player> _EvilPlayers; // list of all Evil players
    [SerializeField] private Button _RollButton;
    [SerializeField] private Text _CurrentSideText; // displays which sides turn it is
    [SerializeField] private bool _DoStartNewGame = false;
    [SerializeField] private Tile _GoodHomeTile; // middle tile for good side
    [SerializeField] private Tile _EvilHomeTile; // middle tile for evil side
    [SerializeField] private GameObject _Camera;
    private Attack _Attack; // Attack script
    private SideType _WinningSide; // winning side, compare to sideType enum to get a result. -1 = no winner
    private float _CameraDisForPiecePlacement = 200.0f;

    private CharacterPiece _currentPiece;
    public CharacterPiece CurrentPiece
    {
        get { return _currentPiece; }
        set
        {
            if (CanSelectePiece)
                DetermineSelecetion(value);
        }
    }

    private TurnManager _turn; // manages a character turn once they hit roll.
    public TurnManager Turn
    {
        get { return _turn; }
    }

    private Player _currentPlayer; // current player
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
                _RollButton.enabled = !value;
                _turnStarted = value;
            }
        }
    }

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
        _WinningSide = SideType.None;
        TurnStarted = false;
        _turn = this.GetComponent<TurnManager>();
        _Attack = GetComponent<Attack>();
        CanSelectePiece = true;
        _RollButton.gameObject.SetActive(false);
    }

    private IEnumerator StartGame()
    {
        if (_DoStartNewGame) // new game
            yield return SetupPiecesForNewGame();
        else // loaded game
            yield return SetupPiecesForContinuedGame();
        while (_WinningSide == SideType.None) // loop till a side has no pieces left
        {
            if (CurrentSide == SideType.Good) // good turn
                yield return GoodPlayersTurn();
            else // evils turn
                yield return EvilPlayersTurn();
        }
    }

    /// <summary>
    /// Start a new game.
    /// </summary>
    private IEnumerator SetupPiecesForNewGame()
    {
        _GoodPlayers = new List<Player>();
        _EvilPlayers = new List<Player>();

        List<CharacterPiece> goodPieces = new List<CharacterPiece>();
        List<CharacterPiece> evilPieces = new List<CharacterPiece>();

        //Create same and dean
        CharacterStat stat = Resources.Load("Characters/Sam", typeof(CharacterStat)) as CharacterStat;
        Debug.Log(stat);
        CharacterPiece sam = CharacterPiece.MakePiece("Sam", stat);
        goodPieces.Add(sam);

        //create evil pieces
        //CharacterPiece death = new CharacterPiece(Resources.Load("Death") as CharacterStat);
        //goodPieces.Add(death);

        _GoodPlayers.Add(new Player(goodPieces, SideType.Good));
        _EvilPlayers.Add(new Player(evilPieces, SideType.Evil));

        GoodSidePlacePieces();
        return null;
    }

    /// <summary>
    /// Loading a game that is already in progress.
    /// </summary>
    private IEnumerator SetupPiecesForContinuedGame()
    {
        foreach (Player play in _GoodPlayers)
        {
            foreach (CharacterPiece piece in play.Pieces)
            {
                if (piece)
                    piece.Stat.SetupStats();
            }
        }

        foreach (Player play in _EvilPlayers)
        {
            foreach (CharacterPiece piece in play.Pieces)
            {
                piece.Stat.SetupStats();
            }
        }
        return null;
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

        _RollButton.gameObject.SetActive(false);
        _RollButton.enabled = false;

        // Means the piece belongs to the current sides turn
        if (CurrentSide == piece.Stat.Side)
        {
            // Piece belongs to Current Player
            if (CurrentPlayer != null && CurrentPlayer.Pieces != null && CurrentPlayer.Pieces.Contains(piece))
            {
                // TODO: Display Current Players piece info **********************

                if (piece.canMove && !_turnStarted) // piece can still roll.
                {
                    _RollButton.gameObject.SetActive(true);
                    _RollButton.enabled = true;
                    piece.DisplaySelected(true);
                    _currentPiece = piece;
                }
            }
        }
        _currentPiece = piece;
    }

    private IEnumerator GoodPlayersTurn()
    {
        foreach (Player play in _GoodPlayers)
        {
            _currentPlayer = play;
            play.SetUpTurn();
            yield return new WaitUntil(() => play.TotalPiecesLeftToMove == 0);
        }
        CurrentSide = SideType.Evil; // it is now evils turn
    }

    private IEnumerator EvilPlayersTurn()
    {;
        foreach (Player play in _EvilPlayers)
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
        foreach (Player play in _GoodPlayers)
        {
            if (play.TotalPieceCount == 0)
                goodDead++;
        }

        if (goodDead == _GoodPlayers.Count)
        {
            _WinningSide = SideType.Evil;
            return true;
        }

        foreach (Player play in _EvilPlayers)
        {
            if (play.TotalPieceCount == 0)
                evilDead++;
        }

        if (evilDead == _EvilPlayers.Count)
        {
            _WinningSide = SideType.Good;
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

    private void GoodSidePlacePieces()
    {
        _Camera.GetComponent<CameraMovement>().target = _GoodHomeTile.transform;
        _Camera.GetComponent<CameraMovement>().distance = _CameraDisForPiecePlacement;

        foreach (CharacterPiece piece in _GoodPlayers[0].Pieces)
        {

        }
    }

    private void DisplayGoodSideHomeTiles()
    {

    }

}
