using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EffectChoiceDescription : MonoBehaviour
{

    [SerializeField] private GameObject Description; // effect description
    [SerializeField] private Button btnUse; // btn for effect use
    public Effect effect; // effect choice
    public event Action<Effect> EffectActivated; // callled when user pickes a effect choice

    private void Start()
    {
        btnUse.onClick.AddListener(BtnClicked);
    }

    private void Update()
    {
        if (effect)
            SetDescription();
    }

    /// <summary>
    /// Display the Description of the effect choice being displayed.
    /// </summary>
    private void SetDescription()
    {
        Description.GetComponentInChildren<Text>().text = effect.Description;
    }

    /// <summary>
    /// Use Button Cliked.
    /// </summary>
    private void BtnClicked()
    {
        EffectActivated.Invoke(effect);
    }
    
}
