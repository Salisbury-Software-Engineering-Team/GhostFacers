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

    public Button BtnActivate;

    // Use this for initialization
    void Start ()
    {
        UpdateDisplay();

    }

    private void Update()
    {
        canActivate(); // display use button if in proper phase.
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

    /// <summary>
    /// Called when the card is being used
    /// </summary>
    public void OnActivate()
    {
        if (_cardName)
            _cardName.ToggleActiavation();
        else
            Debug.Log("Error CardDisplay.OnActivate(): No card found for the display.");
    }

    /// <summary>
    /// Checks each update to see if card can be updated. If yes then displays the use button. If not, then hide.
    /// </summary>
    /// <returns></returns>
    private bool canActivate()
    {
        // If Card activate phase is equal to current phase
        if (_cardName.EffectPhase == GameManager.instance.TurnPhase && !_cardName.DidActivate)
        {
            return BtnActivate.enabled = true;
        }
        else
        {
            return BtnActivate.enabled = false;
        }
    }
}
