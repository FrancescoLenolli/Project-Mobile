using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate void BoughtShip(string shipName); // [!!!] Use event to unlock upgrades.
public delegate void UnlockShip(int index); // Unlock new ship when certain requirements are met.
public delegate void UpdateShipQuantity(int index, int quantity); // Update this ship's quantity when buying one or more.
public delegate void UnlockUpgrades(int shipIndex);

public class Ship : MonoBehaviour
{
    public event BoughtShip eventBoughtShip;
    public event UnlockShip eventUnlockShip;
    public event UpdateShipQuantity eventUpdateShipQuantity;
    public event UnlockUpgrades eventUnlockUpgrades;

    private CurrencyManager currencyManager = null;
    private CanvasBottom canvasBottom = null;

    private List<ShipUpgradesData> listMyUpgrades = new List<ShipUpgradesData>();
    private ShipData.ShipType shipType = ShipData.ShipType.Patrol;
    private int cost = 0;
    private int currencyGain = 0;
    private int currencyGainMultiplier = 0;
    // CurrencyGain increased if more X units are bought.
    private int additionalCurrencyGain = 0;
    private int quantityMultiplier = 0;

    [HideInInspector] public ShipData shipData = null;
    public int quantity = 0;
    public Image imageIcon = null;
    public TextMeshProUGUI textQuantity = null;
    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textCurrencyGain = null;
    public TextMeshProUGUI textAdditionalCurrencyGain = null;
    public TextMeshProUGUI textCost = null;
    public TextMeshProUGUI textQuantityMultiplier = null;
    public Button buttonBuy = null;

    private void Update()
    {

        // [!!!] Add a sprite change.
        // Call this with an event. 
        // When idle currency is updated, when active currency is updated, when buying new ships.
        if(currencyManager)
        buttonBuy.interactable = currencyManager.currency >= cost;

    }

    public void SetValues(ShipData newShipData, int newQuantity)
    {
        currencyManager = CurrencyManager.Instance;

        //[!!!] Use a UI Manager to pass reference to canvasBottom?
        canvasBottom = FindObjectOfType<CanvasBottom>();

        canvasBottom.UpdateModifier += UpdateModifier;
        eventUnlockShip += canvasBottom.panelShips.AddNewShip;
        eventUpdateShipQuantity += canvasBottom.panelShips.UpdateQuantityAt;
        //eventUnlockUpgrades += canvasBottom.panelShipsUpgrades

        shipData = newShipData;
        quantity = newQuantity;
        shipType = newShipData.shipType;

        quantityMultiplier = currencyManager.ReturnModifierValue();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;

        // Multiplier expressed in percentages.
        currencyGainMultiplier = shipData.currencyGainMultiplier / 100;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        textName.text = shipData.shipName;
        imageIcon.sprite = shipData.shipIcon;
        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";

        currencyManager.ChangeCurrencyIdleGain(currencyGain * quantity);
    }

    // Update Values when the Player change the quantity of ships to buy.
    public void UpdateValues()
    {
        quantityMultiplier = currencyManager.ReturnModifierValue();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";
    }

    // Method called from UI button to buy units of a Ship.
    public void Buy()
    {
        int multiplier = quantityMultiplier;

        if (currencyManager.currency >= cost * multiplier)
        {
            currencyManager.currency -= cost;

            // If this is the first time buying this ship, unlock his upgrades.
            if(quantity == 0)
            {
                eventUnlockUpgrades?.Invoke(shipData.index);
            }

            quantity += multiplier;
            currencyManager.ChangeCurrencyIdleGain(currencyGain * multiplier);
            UpdateValues();
            UpdateQuantity(shipData.index, quantity);

            // Notify UpgradesPanel.
            eventBoughtShip?.Invoke(shipData.shipName);

            if (quantity >= shipData.qtToUnlockNextShip) UnlockNextShip(shipData.index);

            // Play sound.
        }
        else
        {
            // Play different sound.
        }
    }

    private void UpdateModifier(int newModifierValue)
    {
        quantityMultiplier = newModifierValue;
        UpdateValues();
    }

    // Unlock next Ship once the right quantity has been reached.
    private void UnlockNextShip(int index)
    {
        eventUnlockShip?.Invoke(index);
    }

    // Update Quantity of this Ship in ShipInfo, that will be saved.
    private void UpdateQuantity(int index, int quantity)
    {
        eventUpdateShipQuantity?.Invoke(index, quantity);
    }

}
