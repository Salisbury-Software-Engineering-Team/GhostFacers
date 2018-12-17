using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[Serializable]
public class CharacterPiece : MonoBehaviour
{
    [SerializeField] private GameObject CharacterVisPlaceholder; 
    [SerializeField] private GameObject Tile; // prefab for tiles

    private List<GameObject> AvaliableMovementTiles; // list of total movement avaliable
    private List<GameObject> BlockedMovementTiles; // tiles with player on them
    [SerializeField] private List<Card> StaggedForDiscard; // list of cards to be discarded at end of turn
    [SerializeField] private Tile _currentTile = null; // current tile piece is at
    public Tile CurrentTile
    {
        get { return _currentTile; }
    }
    public CharacterStat Stat; // stats for the piece 

    public NavMeshAgent Agent; // For character movement
    public GameObject CharacterModel; // For character model
    public GameObject CharacterPlaceHolder; // For character model transfomation template
    public bool canMove; // piece can still roll if true
    public bool doneMove; // once character is done moving;
    public bool StartingPosPlaced = false;

    public event Action StaggedForRollPhase;
    public event Action StaggedForAttackPhase;
    public event Action StaggedForDrawPhase;
    public event Action StaggedForEndPhase;

    public double AttackMultiplier { get { return Stat.AttackDamgeMultiplier; } }

    [SerializeField] private bool _Died;
    public bool Died { get { return _Died; } }

    public event Action<CharacterPiece> DeathHandler;

    private void Awake()
    {
        Destroy(CharacterVisPlaceholder);
        if (Stat != null) // creates character model based off of the stat model chosen
        {
            ChangeCharacterModel();
            Init();
        }
    }

    private void ChangeCharacterModel()
    {
        if (CharacterModel == null)
        {
            GameObject thisModel = Instantiate(Stat.Model, CharacterPlaceHolder.transform.position, CharacterPlaceHolder.transform.rotation, CharacterPlaceHolder.transform) as GameObject;
            CharacterModel = thisModel;
        }
        else
        {
            Destroy(CharacterModel);
            CharacterModel = Instantiate(Stat.Model, CharacterPlaceHolder.transform.position, CharacterPlaceHolder.transform.rotation, CharacterPlaceHolder.transform) as GameObject;
        }
        //***********************TESTIng *******************************Remove Later***********************
        for (int i = 0; i < CharacterModel.transform.childCount; i++)
            CharacterModel.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Stat.PieceColor;
    }

    /// <summary>
    /// Set all vars.
    /// </summary>
    private void Init()
    {
        canMove = false;
        Agent = GetComponent<NavMeshAgent>();
        Agent.enabled = false;
        BlockedMovementTiles = new List<GameObject>();
        AvaliableMovementTiles = new List<GameObject>();
        StaggedForDiscard = new List<Card>();
         doneMove = false;
        _Died = false;
        Stat.Init();
    }

    private void Update()
    {
        if (_Died)
            PieceDied();
    }

    private void OnMouseUp()
    {
        GameManager.instance.CurrentPiece = this;
    }

    /*
     * Display the total movement tiles
     */
    public void DisplayAvaliableMovement(int move)
    {
        if (_currentTile == null) { _currentTile = GetCurrentTile(); } // if Current tile is not set
        GetMovement(_currentTile.gameObject, move);
    }

    /*
     * Find the current tile object that the piece is on.
     */
    private Tile GetCurrentTile()
    {
        float radius = transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider item in hitColliders)
        {
            if (item.gameObject.GetType() == Tile.GetType())
            {
                //Debug.Log("Found Tile");
                //Debug.Log(item.gameObject.transform.parent.gameObject);
                return item.gameObject.transform.parent.gameObject.GetComponent<Tile>();
            }
        }

        // no tile found
        return null;
    }
 
    private void GetMovement(GameObject startTile, int totalMove)
    {
        Queue<GameObject> check = new Queue<GameObject>();
        check.Enqueue(startTile);

        // check if player is in tile location
        Vector3 tempPos;
        Collider[] hitColliders;

        AvaliableMovementTiles.Clear();
        AvaliableMovementTiles.Add(startTile);

        for (int k = 0; k < totalMove; k++)
        {
            Queue<GameObject> temp = new Queue<GameObject>();
            foreach (GameObject current in check)
            {
                // loop for each adj tile
                for (int i = 0; i < current.GetComponent<Tile>().GetAdjTiles().Count; i++)
                {
                    GameObject next = current.GetComponent<Tile>().GetAdjTiles()[i];
                    if (!AvaliableMovementTiles.Contains(next))
                    {
                        // Tile is portal, then skip current tile
                        if (next.GetComponent<Tile>().Portal != null)
                        {
                            temp.Enqueue(next); // enque current tile
                            AvaliableMovementTiles.Add(next); // add curent tile to movment list 
                            next = next.GetComponent<Tile>().Portal; // make current tile = portal destination, then also add that tile
                        }
                        else { next.GetComponent<Tile>().HighlightTile(true); } // not a portal tile

                        temp.Enqueue(next);
                        AvaliableMovementTiles.Add(next);

                        // look for another player piece
                        tempPos = next.transform.position;
                        tempPos.y += transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2; // half the hieght of the piece collider
                        hitColliders = Physics.OverlapSphere(tempPos, transform.GetChild(0).GetComponent<Collider>().bounds.size.x / 4);

                        // tile is blocked
                        if (hitColliders.Length > 0)
                        {
                            BlockedMovementTiles.Add(next);
                        }

                    }
                }
            }
            check = temp;
        }

        // hide blocked tiles. EX tiles with other character pieces on them.
        foreach (GameObject tile in BlockedMovementTiles)
        {
            tile.GetComponent<Tile>().HighlightTile(false);
        }

        //clear Blocked tiles list
        BlockedMovementTiles.Clear();

    }

    /*
     * Hides Highlights and clears movement tile array.
     */
    public void ClearHighlights()
    {
        foreach (GameObject tile in AvaliableMovementTiles)
        {
            tile.GetComponent<Tile>().HighlightTile(false);
        }
        AvaliableMovementTiles.Clear();
    }

    public void SetCurrentTile(GameObject tile)
    {
        _currentTile = tile.GetComponent<Tile>();
    }

    public void DisplaySelected(bool isSelected)
    {
        transform.Find("Selected").gameObject.SetActive(isSelected);
    }

    /// <summary>
    /// Call this once the character piece has finished its turn to reset any vars that 
    /// need to be used again
    /// </summary>
    public void EndTurn()
    {
        Debug.Log("Ending Turn");
        doneMove = false; // reset done move so it can be used in anouth turn
        DisplaySelected(false); // remove highlight
        EmptyStaggedForDiscard();
    }

    /// <summary>
    /// Call this at the begining of the player turn to make sure all vars are propely set for each
    /// players piece.
    /// </summary>
    public void SetupTurn()
    {
        doneMove = false;
        canMove = true;
    }

    /// <summary>
    /// Decrease the characters health by the passed amount. Also Handle any damge reducing abilities here.
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseHealth(int amount)
    {
        if (Stat.CurrentHealth-amount > 0)
        {
            Stat.CurrentHealth = Stat.CurrentHealth - amount;
        }
        else
        {
            Stat.CurrentHealth = 0;
            _Died = true;
        }
    }

    /// <summary>
    /// Handle how a piece dies.
    /// </summary>
    private void PieceDied()
    {
        //_Died = true;

        // TEmpory ***************
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(1000, 1000, 1000); // move piece away from board;
        DeathHandler.Invoke(this);
    }

    public void SetupGame()
    {
        Agent.enabled = true;
        Stat.SetupStats();
    }

    public Card AddCard(Card card)
    {
        Debug.Log("Adding card to character");
        //check if hand is full, if not add card else ask if user wants to remove card.

        //Get the right hand
        //string cardHandName = Enum.GetName(typeof(CardType), card.Deck) + "Hand";
        //List<Card> cardHand = (List<Card>)Stat.GetType().GetProperty(cardHandName).GetValue(Stat, null);

        // Find the right hand to add the card.
        switch (card.DeckType)
        {
            case (CardType.Weapon):
                {
                    Debug.Log("Adding Weapon card to Character");
                    // hand full
                    if (Stat.WeaponHand.Count == Stat.MaxWeapons)
                    {
                        //ChangeCharacterModel later
                        return card;
                    }
                    else
                    {
                        //hand not full
                        Stat.WeaponHand.Add(card);
                        card.OnDraw(this.GetComponent<CharacterPiece>());
                        return null;
                    }
                }
            case (CardType.Help):
                {
                    Debug.Log("Adding Help Card to Character");
                    // hand full
                    if (Stat.HelpHand.Count == Stat.MaxHelp)
                    {
                        // chang later
                        return card;
                    }
                    else
                    {
                        //hand not full
                        Debug.Log("Adding Help card to hand. Piece = " + this.gameObject + "  card = " + card.Name);
                        Stat.HelpHand.Add(card);
                        Debug.Log("Hand size: " + Stat.HelpHand.Count);
                        card.OnDraw(this);
                        return null;
                    }
                }
            default:
                {
                    Debug.Log("Did not find a hand to add card. Card = " + card +"  Card type = " + card.DeckType);
                }
                return null;
        }
    }

    public void RollDice(bool didRoll)
    {
        if (didRoll)
            this.GetComponent<Roll>().RollDice();
        else
            this.GetComponent<Roll>().DontRoll();
    }

    /// <summary>
    /// This will loop thru all the card that need to be discared that the player used in a given turn.
    /// </summary>
    private void EmptyStaggedForDiscard()
    {
        if (StaggedForDiscard != null && StaggedForDiscard.Count > 0)
        {
            for (int i = StaggedForDiscard.Count-1; i >= 0; i--)
            {
                StaggedForDiscard[i].OnDiscard();
                StaggedForDiscard.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Checks to see if the card is in a characters hand. If true, adds card to stagged for discard pile.
    /// </summary>
    /// <param name="card">Card that is being used.</param>
    /// <returns>0 if card added to discard pile, 1 if card not in hand.</returns>
    public int AddToStaggedForDiscard(Card card)
    {
        if (Stat.IsCardInHand(card))
        {
            StaggedForDiscard.Add(card);
            return 0;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// Handles what happens when the curent piece is selected.
    /// </summary>
    public void Selectd()
    {

    }

    /// <summary>
    /// Handlesw what happenjs when the current piece is deselcted.
    /// </summary>
    public void Deselected()
    {
        //if (GameManager.instance.TurnPhase != Phase.Attack)
            //EmptyStaggedForCurrentPhase();
    }

    public void ApplyEffectsStaggedForCurrentPhase()
    {
        switch (GameManager.instance.TurnPhase)
        {
            case Phase.Attack :
                {
                    if (StaggedForAttackPhase != null)
                    {
                        StaggedForAttackPhase.Invoke();
                        StaggedForAttackPhase = null;
                    }
                    break;
                }
            case Phase.Roll:
                {
                    if (StaggedForRollPhase != null)
                    {
                        StaggedForRollPhase.Invoke();
                        StaggedForRollPhase = null;
                    }
                    break;
                }
            case Phase.Draw:
                {
                    if (StaggedForDrawPhase != null)
                    {
                        StaggedForDrawPhase.Invoke();
                        StaggedForDrawPhase = null;
                    }
                    break;
                }
            case Phase.EndTurn:
                {
                    if (StaggedForEndPhase != null)
                    {
                        StaggedForEndPhase.Invoke();
                        StaggedForEndPhase = null;
                    }
                    break;
                }
            default :
                {
                    Debug.Log("Error: Can not apply effect for Phase: " + GameManager.instance.TurnPhase);
                    break;
                }
        }
    }

    /// <summary>
    /// Heal the user for the given amount -1 to fully heal.
    /// </summary>
    /// <param name="amount">Amount to heal. -1 Full heath.</param>
    public void Heal(int amount)
    {
        Stat.Heal(amount);
    }

    /// <summary>
    /// Used to change the attack damge of the character.
    /// </summary>
    /// <param name="value">Precent to increase damge >= 0.0</param>
    public void ModifyAttackDamageMultiplier(double value)
    {
        Stat.ModifyAttackDamageMultiplier(value);
    }
}
