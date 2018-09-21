using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    [SerializeField] private Card _cardName;
    public Card CardName
    {
        get { return _cardName; }
        set
        {
            // card is different.
            if (value != _cardName && value != null)
            {
                _cardName = value;
                UpdateDisplay();
            }
        }
    }

    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;
    public Image back;
    
    public Text healthText; //if card.Health == 0 then display = NULL
    public Text attackText;

    // Use this for initialization
    void Start ()
    {
        UpdateDisplay();

    }

    private void UpdateDisplay()
    {
        string des = "";
        if (_cardName.Summonable)
        {
            des += _cardName.Stat.MaxHelp.ToString();
            des += " Help ";
            des += _cardName.Stat.MaxWeapons.ToString();
            des += " Weapon";
        }

        nameText.text = _cardName.Name;
        if (!_cardName.Summonable || (_cardName.Stat.MaxHelp == 0 && _cardName.Stat.MaxWeapons == 0))
        {
            descriptionText.text = _cardName.Description;
        }
        else
        {
            descriptionText.text = des;
        }

        artworkImage.sprite = _cardName.artwork;
        back.sprite = _cardName.backImage;

        if (!_cardName.Summonable)
        {
            healthText.text = null;
            attackText.text = null;
        }
        else
        {
            healthText.text = _cardName.Stat.StartHealth.ToString();
            attackText.text = _cardName.Stat.StartAttack.ToString();
        }
    }
}
