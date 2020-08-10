using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private CurrencyManager currencyManager = null;
    private int quantity = 0;
    private int cost = 0;
    private int currencyGain = 0;
    private int additionalCurrencyGain = 0; // CurrencyGain increased if more X units are bought

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

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Update()
    {

        buttonBuy.interactable = currencyManager.currency >= cost ? true : false;

    }

    public void SetValues(ShipData newShipData)
    {
        shipData = newShipData;
        quantity = shipData.quantity;
        cost = shipData.cost;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain;

        textName.text = shipData.shipName;
        imageIcon.sprite = shipData.shipIcon;
        textCost.text = $"{cost}"; // * MULTIPLIER, as in buy more than one at a time
        textQuantity.text = $"{quantity}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{currencyGain}/s"; // * MULTIPLIER, as in buy more than one at a time
    }

    public void UpdateValues()
    {

        quantity = shipData.quantity;
        cost = shipData.cost;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain;

        textName.text = shipData.shipName;
        imageIcon.sprite = shipData.shipIcon;
        textCost.text = $"{cost}"; // * MULTIPLIER, as in buy more than one at a time
        textQuantity.text = $"{quantity}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{currencyGain}/s"; // * MULTIPLIER, as in buy more than one at a time
    }

    public void BuyOne()
    {
        if (currencyManager.currency >= cost)
        {
            currencyManager.currency -= cost;
            quantity += 1;
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

}
