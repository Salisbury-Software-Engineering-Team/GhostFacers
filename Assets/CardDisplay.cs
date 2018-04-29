using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public Card card;

    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;
    
    public Text healthText;
    public Text attackText;

	// Use this for initialization
	void Start () {
        nameText.text = card.Name;
        descriptionText.text = card.Description;

        artworkImage.sprite = card.artwork;

        healthText.text = card.Health.ToString();
        attackText.text = card.Attack.ToString();
	}
	
	
}
