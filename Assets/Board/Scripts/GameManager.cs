using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // GameManager object for players to access

    public bool CanSelectePiece; // Determine if piece can be selected
    public int TotalMovement; //testing for movement
    public SideType CurrentSide; // Current sides turn
    public Transform CharacterPiecePrefab;

    public Roll RollDice;

    //Actions
    public event Action<StartingZone, bool> DisplayStartingZone;

    [SerializeField] private List<Player> _GoodPlayers; // lIst of all good players
    [SerializeField] private List<Player> _EvilPlayers; // list of all Evil players
    [SerializeField] private Button _RollButton;
    [SerializeField] private Button _DontRollButton;
    [SerializeField] private Text _CurrentSideText; // displays which sides turn it is
    [SerializeField] private Text _HelpText;
    [SerializeField] private bool _DoStartNewGame = false;
    [SerializeField] private Tile _GoodHomeTile; // middle tile for good side
    [SerializeField] private Tile _EvilHomeTile; // middle tile for evil side
    [SerializeField] private GameObject _Camera;
    [SerializeField] private GameObject _PlayableGoodPiecesContainer;
    [SerializeField] private GameObject _PlayableEvilPiecesContainer;
    private Attack _Attack; // Attack script
    private SideType _WinningSide; // winning side, compare to sideType enum to get a result. -1 = no winner
    private float _CameraDisForPiecePlacement = 200.0f;

    // get current turn phase
    [SerializeField] private Phase _turnPhase;
    public Phase TurnPhase { get { return _turnPhase; } }

    [SerializeField] private CharacterPiece c;
    [SerializeField] private CharacterPiece _currentPiece
    {
        set
        {
            if (c != value)
            {
                if (c)
                {
                    c.Deselected();
                    c = value;
                    if (c)
                        c.Selectd();
                }
                else
                    c = value;
            }
        }
        get { return c; }
    }
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
                _DontRollButton.enabled = !value;
                _turnStarted = value;
                if (!value)
                {
                   MoveCameraToNextPiece();
                }
            }
        }
    }

    private bool _gameStarted;
    public bool GameStarted { get { return _gameStarted; } }

    private Deck _cardDeck;
    public Deck CardDeck { get { return _cardDeck; } }

    private Action<Card> ChoiceEffectHandler;
    private Card _choiceEffect;
    public Card ChoiceEffect
    {
        set
        {
            _choiceEffect = value;
            if (_choiceEffect)
                ChoiceEffectHandler.Invoke(_choiceEffect);
        }
        get { return _choiceEffect; }
    }

	private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
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
        if (GameStarted && CheckForWinner())
            WinnerFound();

        if (_turn.TurnPhase != Phase.None)
            _turnPhase = _turn.TurnPhase;
    }

    private void Init()
    {
        _gameStarted = false;
        _WinningSide = SideType.None;
        TurnStarted = false;
        _turn = this.GetComponent<TurnManager>();
        _Attack = GetComponent<Attack>();
        CanSelectePiece = false;
        _RollButton.gameObject.SetActive(false);
        _DontRollButton.gameObject.SetActive(false);
        _turnPhase = Phase.Roll;
        RollDice = this.GetComponent<Roll>();
        _cardDeck = this.GetComponent<Deck>();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1); // wait till everything is done loading
        if (_DoStartNewGame) // new game
            yield return SetupPiecesForNewGame();
        else // loaded game
            yield return SetupPiecesForContinuedGame();
        Debug.Log("Done Setingup the Game");
        c = null;
        _currentPiece = null;
        _gameStarted = true;
        CanSelectePiece = true;

        // move camera to current sides pieces
        if (CurrentSide == SideType.Good)
            _Camera.GetComponent<CameraMovement>().target = _GoodHomeTile.transform;
        else
            _Camera.GetComponent<CameraMovement>().target = _GoodHomeTile.transform;

        _HelpText.text = "Select Piece to Move";

        while (_WinningSide == SideType.None) // loop till a side has no pieces left
        {
            _currentPiece = null;
            if (CurrentSide == SideType.Good) // good turn
            {
                yield return GoodPlayersTurn();
            }
            else // evils turn
            {
                yield return EvilPlayersTurn();
            }
        }
        Debug.Log("Done Game" + _WinningSide);
    }

    /// <summary>
    /// Start a new game.
    /// </summary>
    private IEnumerator SetupPiecesForNewGame()
    {
        _GoodPlayers.Clear();
        _EvilPlayers.Clear();

        List<CharacterPiece> goodPieces = new List<CharacterPiece>();
        List<CharacterPiece> evilPieces = new List<CharacterPiece>();

        // get all te good starting pieces
        foreach (CharacterPiece piece in _PlayableGoodPiecesContainer.transform.Find("Starting Pieces").GetComponentsInChildren<CharacterPiece>())
        {
            goodPieces.Add(piece);
        }

        // get all the evil starting pieces
        foreach (CharacterPiece piece in _PlayableEvilPiecesContainer.transform.Find("Starting Pieces").GetComponentsInChildren<CharacterPiece>())
        {
            evilPieces.Add(piece);
        }

        _GoodPlayers.Add(new Player(goodPieces, SideType.Good));
        _EvilPlayers.Add(new Player(evilPieces, SideType.Evil));

        yield return GoodSidePlacePieces();
        Debug.Log("Done placing Good");
        yield return EvilSidePlacePieces();
        Debug.Log("Done placing Evil");

        // good goes first
        CurrentSide = SideType.Good;
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
        if (piece)
            SetCurrentPiece(piece); // select piece
        else
            _currentPiece = null;

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
        _DontRollButton.gameObject.SetActive(false);
        _DontRollButton.enabled = false;
        _RollButton.gameObject.SetActive(false);
        _RollButton.enabled = false;
        // Means the piece belongs to the current sides turn
        if (CurrentSide == piece.Stat.Side)
        {
            // Piece belongs to Current Player
            if (CurrentPlayer != null && CurrentPlayer.Pieces != null && CurrentPlayer.Pieces.Contains(piece))
            {
                if (piece.canMove && !_turnStarted) // piece can still roll.
                {
                    _RollButton.gameObject.SetActive(true);
                    _RollButton.enabled = true;
                    _DontRollButton.gameObject.SetActive(true);
                    _DontRollButton.enabled = true;
                    piece.DisplaySelected(true);
                    _currentPiece = piece;
                }
            }
        }
        _currentPiece = piece;
    }

    private IEnumerator GoodPlayersTurn()
    {
        Debug.Log("Good players turn");
        foreach (Player play in _GoodPlayers)
        {
            _Camera.GetComponent<CameraMovement>().target = play.Pieces[0].transform;
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
            _Camera.GetComponent<CameraMovement>().target = play.Pieces[0].transform;
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
        _HelpText.text = "Winner Found = " + ((_WinningSide == SideType.Good) ? "Good" : "Evil");
        _CurrentSideText.text = "Winner Found = " + ((_WinningSide == SideType.Good) ? "Good" : "Evil");
        _CurrentSideText.fontSize = 40;

    }

    private IEnumerator GoodSidePlacePieces()
    {
        CurrentSide = SideType.Good;
        // Camera show the good side starting location
        _Camera.GetComponent<CameraMovement>().target = _GoodHomeTile.transform;
        _Camera.GetComponent<CameraMovement>().distance = _CameraDisForPiecePlacement;

        // highlight the good side starting location.
        DisplayStartingZone.Invoke(StartingZone.Good, true);
        // Player must place each of there pieces in the starting location
        foreach (CharacterPiece piece in _GoodPlayers[0].Pieces)
        {
            _HelpText.text = "Select Starting Location For " + piece.Stat.Name;
            _currentPiece = piece;
            yield return new WaitUntil(() => piece.CurrentTile != null);
            piece.SetupGame();
        }
        //hide good starting highlights
        DisplayStartingZone.Invoke(StartingZone.Good, false);
    }

    private IEnumerator EvilSidePlacePieces()
    {
        CurrentSide = SideType.Evil; // it is now evils turn
        // Camera show the good side starting location
        _Camera.GetComponent<CameraMovement>().target = _EvilHomeTile.transform;
        _Camera.GetComponent<CameraMovement>().distance = _CameraDisForPiecePlacement;

        // highlight the good side starting location.
        DisplayStartingZone.Invoke(StartingZone.Evil, true);
        // Player must place each of there pieces in the starting location
        foreach (CharacterPiece piece in _EvilPlayers[0].Pieces)
        {
            _HelpText.text = "Select Starting Location For " + piece.Stat.Name;
            _currentPiece = piece;
            yield return new WaitUntil(() => piece.CurrentTile != null);
            piece.SetupGame();
        }
        // hide evil starting highlights
        DisplayStartingZone.Invoke(StartingZone.Evil, false);
    }

    private void MoveCameraToNextPiece()
    {
        foreach (CharacterPiece piece in _currentPlayer.Pieces)
        {
            if(piece.canMove)
            {
                Debug.Log(piece);
                _Camera.GetComponent<CameraMovement>().target = piece.transform;
                break;
            }
        }
    }

    public void CurrentCharacterRoll(bool didRoll)
    {
        _currentPiece.RollDice(didRoll);
    }

    public void CurrentCharacterMovement()
    {

    }

    public void CurrentCharacterAttack()
    {

    }

    /// <summary>
    /// Only allow one listener to be added to the effectchoicehandler
    /// </summary>
    /// <param name="action"></param>
    public void AddEffectChoiceListener(Action<Card> action)
    {
        if (ChoiceEffectHandler == null)
            ChoiceEffectHandler += (c) => action(c);
    }
}
