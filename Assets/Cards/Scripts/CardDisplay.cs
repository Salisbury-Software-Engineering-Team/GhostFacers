using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public Card card;

    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;
    public Image back;
    
    public Text healthText; //if card.Health == 0 then display = NULL
    public Text attackText;

    // Use this for initialization
    void Start () {
        
        string des = "";
        des += card.Help.ToString();
        des += " Help ";
        des += card.Weapon.ToString();
        des += " Weapon";

        nameText.text = card.Name;
        if (!card.Summonable || (card.Help == 0 && card.Weapon == 0))
        {
            descriptionText.text = card.Description;
        }
        else
        {
            descriptionText.text = des;
        }

        artworkImage.sprite = card.artwork;
        back.sprite = card.backImage;

        if(card.Health == 0 && card.Attack == 0)
        {
            healthText.text = null;
            attackText.text = null;
        }
        else
        {
            healthText.text = card.Health.ToString();
            attackText.text = card.Attack.ToString();
        }

    }

    private void OnValidate()
    {
        back.color = card.Type.ButtonColor;
    }

}
