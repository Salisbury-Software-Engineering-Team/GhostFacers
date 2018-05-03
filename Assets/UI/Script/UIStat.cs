using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    public GameObject NamePanel;
    public GameObject HealthPanel;
    public GameObject AttackPanel;
    [Space]
    public GameObject NamePanelTurn;
    public GameObject HealthPanelTurn;
    public GameObject AttackPanelTurn;
    [Space]
    public Sprite HealthSprite;
    public Sprite AttackSprite;
    public GameObject ImagePrefab;

    private CharacterPiece CurrentPiece;
    private int CurrentPieceHealth;
    private int CurrentPieceAttack;

    private CharacterPiece CurrentPieceTurn;
    private int CurrentPieceTurnHealth;
    private int CurrentPieceTurnAttack;

    private bool TurnStarted;


    private void LateUpdate()
    {
        if (DidCurrentPieceChange())
            DisplayCharacterStat();
        if (DidCurrentPieceTurnChange())
            DisplayCharacterStatTurn();
    }

    private void Start()
    {
        Init();
        if (GameManager.instance.CurrentPiece != null)
        {
            CurrentPiece = GameManager.instance.CurrentPiece;
        }

        Debug.Log(CurrentPiece);

        // Hide Panels bc no character is selected
        AttackPanel.SetActive(false);
        HealthPanel.SetActive(false);
    }

    private void Init()
    {
        TurnStarted = false;
        CurrentPieceTurn = null;

        // hide current seleceted
        NamePanel.SetActive(false);
        AttackPanel.SetActive(false);
        HealthPanel.SetActive(false);

        //hide current turn
        NamePanelTurn.SetActive(true);
        AttackPanelTurn.SetActive(true);
        HealthPanelTurn.SetActive(true);
        NamePanelTurn.SetActive(false);
        AttackPanelTurn.SetActive(false);
        HealthPanelTurn.SetActive(false);
    }

    private void DisplayCharacterStat()
    {
        // current piece is not nul and not equal to current piece turn
        if (CurrentPiece != null && CurrentPiece != CurrentPieceTurn)
        {
            NamePanel.transform.GetChild(0).GetComponent<Text>().text = CurrentPiece.Stat.name;
            DisplayHealth(CurrentPiece, HealthPanel);
            DisplayAttack(CurrentPiece, AttackPanel);
            // Turn on the panels
            NamePanel.SetActive(true);
            AttackPanel.SetActive(true);
            HealthPanel.SetActive(true);
        }
        else
        {
            // Hide Panels bc no character is selected
            NamePanel.SetActive(false);
            AttackPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
    }

    private bool DidCurrentPieceChange()
    {
        if (GameManager.instance.CurrentPiece != CurrentPiece) // current piece seleced changed
        {
            CurrentPiece = GameManager.instance.CurrentPiece;
            CurrentPieceHealth = (CurrentPiece != null) ? CurrentPiece.Stat.HealthLeft : 0;
            CurrentPieceAttack = (CurrentPiece != null) ? CurrentPiece.Stat.Attack : 0;
            return true;
        }
        else if (CurrentPiece != null) // current piece did not change and is not null
        {
            if (CurrentPiece.Stat.HealthLeft != CurrentPieceHealth) // health of current piece changed
            {
                return true;
            }
            else if (CurrentPiece.Stat.Attack != CurrentPieceAttack) // attack of current piece changed
            {
                return true;
            }
        }
        return false;
}

    /// <summary>
    /// Displays the current selced pieces health bar
    /// </summary>
    private void DisplayHealth(CharacterPiece piece, GameObject healthPanel)
    {
        int health = CurrentPiece.Stat.HealthLeft;
        int currentChildCount = HealthPanel.transform.childCount; // get the current amount of health being displayed

        // need to add or remove hearts
        if (currentChildCount != health)
        {
            GameObject image;

            // add hearts
            if (health > currentChildCount)
            {
                for (int i = currentChildCount; i < health; i++) // add the difference of hearts
                {
                    image = Instantiate(ImagePrefab, HealthPanel.transform);
                    image.GetComponent<Image>().sprite = HealthSprite;
                }
            }
            else // remove hearts
            {
                for (int i = 0; i < currentChildCount - health; i++) // remove the difference of hearts
                {
                    Destroy(HealthPanel.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Displays the current selected pieces attack bar.
    /// </summary>
    private void DisplayAttack(CharacterPiece piece, GameObject attackPanel)
    {
        int attack = piece.Stat.Attack; // attack points
        int currentChildCount = attackPanel.transform.childCount; // get the current amount of health being displayed

        // need to add or remove swords
        if (currentChildCount != attack)
        {
            GameObject image;

            // add swords
            if (attack > currentChildCount)
            {
                for (int i = currentChildCount; i < attack; i++) // add the difference of swords
                {
                    image = Instantiate(ImagePrefab, attackPanel.transform);
                    image.GetComponent<Image>().sprite = AttackSprite;
                }
            }
            else // remove swords
            {
                for (int i = 0; i < currentChildCount - attack; i++) // remove the difference of swords
                {
                    Destroy(attackPanel.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    private void DisplayCharacterStatTurn()
    {
        NamePanel.transform.GetChild(0).GetComponent<Text>().text = CurrentPieceTurn.Stat.name;
        DisplayHealth(CurrentPieceTurn, HealthPanelTurn);
        DisplayAttack(CurrentPieceTurn, AttackPanelTurn);

        // Turn on the panels
        NamePanelTurn.SetActive(true);
        AttackPanelTurn.SetActive(true);
        HealthPanelTurn.SetActive(true);
    }

    private bool DidCurrentPieceTurnChange()
    {
        // Character began turn
        if (GameManager.instance.TurnStarted)
        {
            if (TurnStarted != true) // turn just started
            {
                CurrentPieceTurn = GameManager.instance.Turn.Piece;
                TurnStarted = true;
                CurrentPieceTurnHealth = (CurrentPieceTurn != null) ? CurrentPieceTurn.Stat.HealthLeft : 0;
                CurrentPieceTurnAttack = (CurrentPieceTurn != null) ? CurrentPieceTurn.Stat.Attack : 0;
                return true;
            }
            else if (CurrentPieceTurnHealth != CurrentPieceTurn.Stat.HealthLeft || CurrentPieceTurnAttack != CurrentPieceTurn.Stat.Attack)
            {
                // attack or health has changed of current piece;
                return true;
            }

        }
        else if (TurnStarted == true) // hide current piece turn bc turn over
        {
            TurnStarted = false;
            CurrentPieceTurn = null;
            NamePanelTurn.SetActive(false);
            AttackPanelTurn.SetActive(false);
            HealthPanelTurn.SetActive(false);
        }

        return false;
    }
}
