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
    
    public Text healthText;
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

        healthText.text = card.Health.ToString();
        attackText.text = card.Attack.ToString();

        //used to change card colors
        //card.artwork.color = new Color(255, 0, 0, 1);

        back.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
        switch (card.Deck)
        {
            case cardType.Angel:
                back.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
                break;
            case cardType.Monster:
                back.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 1);
                break;
            case cardType.Demon:
                back.GetComponent<SpriteRenderer>().color = new Color(173, 96, 0, 1);
                break;
            case cardType.Help:
                back.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 1);
                break;
            case cardType.Weapon:
                back.GetComponent<SpriteRenderer>().color = new Color(255, 0, 255, 1);
                break;
            default:
                back.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1);
                break;
        }

        back.GetComponent<SpriteRenderer>().color = Color.red;
    }

}
