using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasMain : MonoBehaviour
{
    CurrencyManager currencyManager;

    public TextMeshProUGUI textCurrency;

    private void Awake()
    {
        
    }

    private void Start()
    {
        currencyManager = CurrencyManager.Instance; //[!!!] Place in Awake, need "bootstrap" Scene to initialise Managers first
        currencyManager.UpdateCurrencyText += UpdateCurrencyText;
    }

    private void UpdateCurrencyText(long value)
    {
        textCurrency.text = value.ToString(); 
    }

    public void ActiveCurrencyGain()
    {
        currencyManager.AddActiveCurrency();
    }
}
