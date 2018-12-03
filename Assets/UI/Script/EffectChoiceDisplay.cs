using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectChoiceDisplay : MonoBehaviour
{
    private GameManager ins;
    public GameObject CardDisplay;
    private Card _card;

    [SerializeField] private GameObject ChoiceEffectsPannel;
    [SerializeField] private GameObject ChoiceEffectPannelPreFab;

    public Card TestingCard;

	// Use this for initialization
	private void Start ()
    {
        this.gameObject.SetActive(false);
	}

    private void Awake()
    {
        ins = GameManager.instance;
        ins.AddEffectChoiceListener(HandleEffectChoice);

        //TESTING*********************************
        HandleEffectChoice(TestingCard);
        //****************************************
    }

    // Update is called once per frame
    private void Update ()
    {
		
	}

    /// <summary>
    /// Shows the avaliable choices for the user to pick.
    /// </summary>
    /// <param name="c"></param>
    public void HandleEffectChoice(Card c)
    {
        this.gameObject.SetActive(true);
        _card = c;
        CardDisplay.GetComponent<CardDisplay>().CardName = _card;
        DisplayChoices();
    }

    /// <summary>
    /// Display the different effect choices for the users to pick.
    /// </summary>
    private void DisplayChoices()
    {
        List<Effect> effects = ((ChoiceEffect)_card.CardEffect).InstanceEffects; // list of effects that need to be displayed
        int numEffectCurrentlyDisplayed = ChoiceEffectsPannel.transform.childCount; //get number of current created effect choices

        // Create more panels for effects to be displayed
        if (numEffectCurrentlyDisplayed < effects.Count)
        {
            for (int i = numEffectCurrentlyDisplayed; i < effects.Count; i++)
            {
                Instantiate(ChoiceEffectPannelPreFab, ChoiceEffectsPannel.transform);
            }
        }
        else if (numEffectCurrentlyDisplayed > effects.Count)
        {
            //Hide extra panels
            for (int i = numEffectCurrentlyDisplayed; i > effects.Count; i--)
            {
                ChoiceEffectsPannel.transform.GetChild(i-1).gameObject.SetActive(false);
            }

        }

        // Display the description for each choice that needs to be displayed
        for (int i = 0; i < effects.Count; i++)
        {
            EffectChoiceDescription choice = ChoiceEffectsPannel.transform.GetChild(i).GetComponent<EffectChoiceDescription>();
            choice.effect = effects[i];
            choice.EffectActivated += (e) => ((ChoiceEffect)_card.CardEffect).EffectPicked(e);
            choice.EffectActivated += (e) => EffectPicked(e);
        }
    }

    /// <summary>
    /// User picked a choice.
    /// </summary>
    /// <param name="e"></param>
    public void EffectPicked(Effect e)
    {
        this.gameObject.SetActive(false);
        ResetVars();
    }

    private void ResetVars()
    {
        _card = null;
    }
}
