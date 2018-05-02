using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    public GameObject AttackPanel;
    public GameObject HealthPanel;
    public Sprite HealthSprite;
    public Sprite AttackSprite;
    public GameObject ImagePrefab;

    private CharacterPiece CurrentPiece;
    private int CurrentPieceHealth;
    private int CurrentPieceAttack;


    private void LateUpdate()
    {
        if (DidCurrentPieceChange())
            DisplayCharacterStat();
    }

    private void Start()
    {
        if (GameManager.instance.CurrentPiece != null)
        {
            CurrentPiece = GameManager.instance.CurrentPiece;
        }

        Debug.Log(CurrentPiece);

        // Hide Panels bc no character is selected
        AttackPanel.SetActive(false);
        HealthPanel.SetActive(false);
    }

    private void DisplayCharacterStat()
    {
        if (CurrentPiece != null)
        {
            DisplayHealth();
            DisplayAttack();
            // Turn on the panels
            AttackPanel.SetActive(true);
            HealthPanel.SetActive(true);
        }
        else
        {
            // Hide Panels bc no character is selected
            AttackPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
    }

    private bool DidCurrentPieceChange()
    {
        if (GameManager.instance.CurrentPiece != CurrentPiece) // current piece seleced changed
        {
            CurrentPiece = GameManager.instance.CurrentPiece;
            CurrentPieceHealth = CurrentPiece.Stat.HealthLeft;
            CurrentPieceAttack = CurrentPiece.Stat.Attack;
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

    private void DisplayHealth()
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

            // resize the width to mathc height
            RectTransform trans = HealthPanel.GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(health * trans.sizeDelta.y, trans.sizeDelta.y);
        }
    }

    private void DisplayAttack()
    {
        int attack = CurrentPiece.Stat.Attack;
        int currentChildCount = AttackPanel.transform.childCount; // get the current amount of health being displayed

        // need to add or remove hearts
        if (currentChildCount != attack)
        {
            GameObject image;

            // add hearts
            if (attack > currentChildCount)
            {
                for (int i = currentChildCount; i < attack; i++) // add the difference of hearts
                {
                    image = Instantiate(ImagePrefab, AttackPanel.transform);
                    image.GetComponent<Image>().sprite = AttackSprite;
                }
            }
            else // remove hearts
            {
                for (int i = 0; i < currentChildCount - attack; i++) // remove the difference of hearts
                {
                    Destroy(AttackPanel.transform.GetChild(i).gameObject);
                }
            }

            // resize the width to mathc height
            RectTransform trans = AttackPanel.GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(attack * trans.sizeDelta.y, trans.sizeDelta.y);
        }
    }
}
