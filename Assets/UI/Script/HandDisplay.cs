using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandDisplay : MonoBehaviour
{
    private CharacterPiece _Piece;
    private GameManager gm;

    public GameObject HandWeaponContainer;
    public GameObject HandHelpContainer;

    public GameObject WeaponText;
    public GameObject HelpText;

    private void Start()
    {
        gm = GameManager.instance;
        HideAll();
    }

    private void Update()
    {
        /* ****************TODO Change later
        // piece selected changed
        if (gm.CurrentPiece != _Piece)
        {
            _Piece = gm.CurrentPiece;
            UpdateDisplay();
        }*/
        _Piece = gm.CurrentPiece;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // a piece is selected.
        if (_Piece)
        {
            DisplayHandForAlly();
            if (_Piece.Stat.Side == gm.CurrentSide)
            {
                //DisplayHandForAlly();
            }
        }
        else
            HideAll();
    }

    private void DisplayHandForAlly()
    {
        List<Card> weaponHand = _Piece.Stat.WeaponHand;
        List<Card> helpHand = _Piece.Stat.HelpHand;
        int currentWeapon = weaponHand.Count;
        int currentHelp = helpHand.Count;

        int currentWeaponDisplayed = HandWeaponContainer.transform.childCount;
        int currentHelpDisplayed = HandHelpContainer.transform.childCount;

        int i = 0;

        if (currentWeapon > 0)
            WeaponText.gameObject.SetActive(true);
        else
            WeaponText.gameObject.SetActive(false);

        if (currentHelp > 0)
            HelpText.gameObject.SetActive(true);
        else
            HelpText.gameObject.SetActive(false);

        //Display weapon ards in hand
        while (i < currentWeapon)
        {
            HandWeaponContainer.transform.GetChild(i).gameObject.SetActive(true);
            HandWeaponContainer.transform.GetChild(i).GetComponent<CardDisplay>().CardName = weaponHand[i];
            i++;
        }

        //Hide other weaponn cards
        while (i < currentWeaponDisplayed)
        {
            HandWeaponContainer.transform.GetChild(i).gameObject.SetActive(false);
            i++;
        }
        
        i = 0;
        //Display weapon ards in hand
        while (i < currentHelp)
        {
            HandHelpContainer.transform.GetChild(i).gameObject.SetActive(true);
            HandHelpContainer.transform.GetChild(i).GetComponent<CardDisplay>().CardName = helpHand[i];
            i++;
        }

        //Hide other weaponn cards
        while (i < currentHelpDisplayed)
        {
            HandHelpContainer.transform.GetChild(i).gameObject.SetActive(false);
            i++;
        }
    }

    private void DisplayHandForEnemy()
    {

    }

    private void HideAll()
    {
        for (int i = 0; i < HandWeaponContainer.transform.childCount; i++)
        {
            HandWeaponContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < HandHelpContainer.transform.childCount; i++)
        {
            HandHelpContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
        WeaponText.gameObject.SetActive(false);
        HelpText.gameObject.SetActive(false);
    }
}
