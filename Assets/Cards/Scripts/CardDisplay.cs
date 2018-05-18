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
        des += card.Stat.MaxHelp.ToString();
        des += " Help ";
        des += card.Stat.MaxWeapons.ToString();
        des += " Weapon";

        nameText.text = card.Name;
        if (!card.Summonable || (card.Stat.MaxHelp == 0 && card.Stat.MaxWeapons == 0))
        {
            descriptionText.text = card.Description;
        }
        else
        {
            descriptionText.text = des;
        }

        artworkImage.sprite = card.artwork;
        back.sprite = card.backImage;

        if(card.Stat.StartHealth == 0 && card.Stat.StartAttack == 0)
        {
            healthText.text = null;
            attackText.text = null;
        }
        else
        {
            healthText.text = card.Stat.StartHealth.ToString();
            attackText.text = card.Stat.StartAttack.ToString();
        }

    }

    /*private void OnValidate()
    {
        Debug.Log(back);
        Debug.Log(back.color);
        Debug.Log(card);
        Debug.Log(card.Type);
        Debug.Log(card.Type.ButtonColor);
        back.color = card.Type.ButtonColor;
    }*/

}
