﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private CurrencyManager currencyManager = null;
    private CanvasBottom canvasBottom = null; 
    private int quantity = 0;
    private int cost = 0;
    private int currencyGain = 0;
    private int additionalCurrencyGain = 0; // CurrencyGain increased if more X units are bought
    private int quantityMultiplier = 0;

    // [!!!] ShipManager passes a list of shipData to the Canvas, the Canvas assign every shipData with isAvailable TRUE, instantiate the Ship UI Prefab, that Initialise his values
    [HideInInspector] public ShipData shipData = null;
    public Image imageIcon = null;
    public TextMeshProUGUI textQuantity = null;
    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textCurrencyGain = null;
    public TextMeshProUGUI textAdditionalCurrencyGain = null;
    public TextMeshProUGUI textCost = null;
    public TextMeshProUGUI textQuantityMultiplier = null;
    public Button buttonBuy = null;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Start()
    {
        canvasBottom = FindObjectOfType<CanvasBottom>();

        canvasBottom.UpdateModifier += UpdateModifier;
    }

    private void Update()
    {

        buttonBuy.interactable = currencyManager.currency >= cost ? true : false;

    }

    public void SetValues(ShipData newShipData)
    {
        shipData = newShipData;
        quantityMultiplier = currencyManager.ReturnModifierValue();
        quantity = shipData.quantity;
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        textName.text = shipData.shipName;
        imageIcon.sprite = shipData.shipIcon;
        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";
    }

    public void UpdateValues()
    {

        quantity = shipData.quantity;
        quantityMultiplier = currencyManager.ReturnModifierValue();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        textName.text = shipData.shipName;
        imageIcon.sprite = shipData.shipIcon;
        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";
    }

    public void BuyOne()
    {
        if (currencyManager.currency >= cost)
        {
            currencyManager.currency -= cost;
            quantity += quantityMultiplier;
            currencyManager.ChangeCurrencyIdleGain(currencyGain);
            UpdateSOValues();
            UpdateValues(); // [!!!] No need to Modify some variables, TO MODIFY
        }
    }

    private void UpdateSOValues()
    {
        shipData.quantity = quantity;
        shipData.cost = cost;
        //shipData.currencyGain = currencyGain; // [!!!] Some Upgrades to Improve currencyGain?
    }

    private void UpdateModifier(int newModifierValue)
    {
        quantityMultiplier = newModifierValue;
        UpdateValues();
        //Debug.Log($"Modifier is now {quantityMultiplier}");
    }

}
