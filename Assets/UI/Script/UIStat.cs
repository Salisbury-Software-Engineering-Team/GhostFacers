using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStat : MonoBehaviour
{
    public GameObject AttackPanel;
    public GameObject HealthPanel;
    public Sprite HealthSprite;
    public Sprite AttackSprite;

    private CharacterPiece CurrentPiece;


    private void Update()
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
    }

    private void OnValidate()
    {
        RectTransform trans = HealthPanel.GetComponent<RectTransform>();
        int healthCount = HealthPanel.transform.childCount;

        trans.sizeDelta = new Vector2(healthCount * trans.sizeDelta.y, trans.sizeDelta.y);

        Debug.Log("Changed Size");
    }

    private void DisplayCharacterStat()
    {
        if (CurrentPiece != null)
        {
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
            return true;
        }
        else
        {
            return false;
        }

    }
}
