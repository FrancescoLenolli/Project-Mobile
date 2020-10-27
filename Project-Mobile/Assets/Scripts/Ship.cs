﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate void BoughtShip(string shipName); // [!!!] Use event to unlock upgrades.
public delegate void UnlockShip(int index); // Unlock new ship when certain requirements are met.
public delegate void UpdateShipQuantity(int index, int quantity); // Update this ship's quantity when buying one or more.
public delegate void UpdateShipModifier(int index, int modifier); // Update this ship's idleGain modifier when buying an upgrade.
public delegate void UnlockUpgrades(ShipData.ShipType myType);
public delegate void UpdateIdleGain();

public class Ship : MonoBehaviour
{
    public event BoughtShip eventBoughtShip;
    public event UnlockShip eventUnlockShip;
    public event UpdateShipQuantity eventUpdateShipQuantity;
    public event UpdateShipModifier eventUpdateShipModifier;
    public event UnlockUpgrades eventUnlockUpgrades;
    public event UpdateIdleGain eventUpdateIdleGain;

    private CurrencyManager currencyManager = null;
    private CanvasBottom canvasBottom = null;

    private ShipData.ShipType shipType = ShipData.ShipType.Patrol;
    private int index = 0;
    private string shipName = "";
    private Sprite shipIcon = null;
    private int cost = 0;
    private int currencyGain = 0;
    // How much currency does this ship gain every second.
    private int idleGain = 0;
    // CurrencyGain increased if more X units are bought.
    private int additionalCurrencyGain = 0;
    private int quantityMultiplier = 0;
    // CurrencyGain is increased by this percentage when buying upgrades.
    private int productionMultiplier = 0;
    private int qtToUnlockNextShip = 0;

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

    public void SetValues(ShipData newShipData, int newQuantity, int newMultiplier)
    {
        currencyManager = CurrencyManager.Instance;
        //[!!!] Use a UI Manager to pass reference to canvasBottom?
        canvasBottom = FindObjectOfType<CanvasBottom>();

        canvasBottom.eventUpdateQuantityModifier += UpdateQuantityModifier;
        eventUnlockShip += canvasBottom.panelShips.AddNewShip;
        eventUpdateShipQuantity += canvasBottom.panelShips.UpdateQuantityAt;
        eventUpdateShipModifier += canvasBottom.panelShips.UpdateModifierAt;
        eventUnlockUpgrades += canvasBottom.panelShipsUpgrades.UnlockUpgrades;
        eventUpdateIdleGain += UpdateIdleGain;

        quantity = newQuantity;
        productionMultiplier = newMultiplier;
        shipData = newShipData;
        shipType = shipData.shipType;
        index = shipData.index;
        shipName = shipData.shipName;
        shipIcon = shipData.shipIcon;
        cost = shipData.cost;
        currencyGain = shipData.currencyGain;
        qtToUnlockNextShip = shipData.qtToUnlockNextShip;

        quantityMultiplier = currencyManager.GetQuantityToBuy();
        cost *= quantityMultiplier;

        additionalCurrencyGain = currencyGain * quantityMultiplier;

        currencyManager.IncreaseCurrencyIdleGain(ReturnIdleGain());

        textName.text = shipName;
        imageIcon.sprite = shipIcon;
        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{ReturnIdleGain()}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";
    }

    // Update Values when the Player change the quantity of ships to buy.
    public void UpdateValues()
    {
        quantityMultiplier = currencyManager.GetQuantityToBuy();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        UpdateIdleGain();

        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{ReturnIdleGain()}/s";
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
                eventUnlockUpgrades?.Invoke(shipType);
            }

            // Recalculate IdleGain according to new Quantity.
            UpdateIdleGainForQuantity(multiplier);
            UpdateValues();
            UpdateQuantity(index, quantity);

            // Notify UpgradesPanel.
            eventBoughtShip?.Invoke(shipName);

            if (quantity >= shipData.qtToUnlockNextShip) UnlockNextShip(index);

            // Play sound.
        }
        else
        {
            // Play different sound.
        }
    }

    public void UpdateIdleGain(int multiplier)
    {
        currencyManager.DecreaseCurrencyIdleGain(idleGain);

        productionMultiplier += multiplier;
        eventUpdateIdleGain?.Invoke();

        currencyManager.IncreaseCurrencyIdleGain(idleGain);

        UpdateValues();
        UpdateMultiplier(shipData.index, productionMultiplier);
    }

    public void UpdateIdleGainForQuantity(int additionalQuantity)
    {
        currencyManager.DecreaseCurrencyIdleGain(idleGain);

        quantity += additionalQuantity;
        eventUpdateIdleGain?.Invoke();

        currencyManager.IncreaseCurrencyIdleGain(idleGain);
    }

    // Update how many units of this ship the player want to buy
    private void UpdateQuantityModifier(int newModifierValue)
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

    private void UpdateMultiplier(int index, int multiplier)
    {
        eventUpdateShipModifier?.Invoke(index, multiplier);
    }

    // Recalculate the idleGain of this Ship;
    private void UpdateIdleGain()
    {
        idleGain = currencyGain * quantity + ((currencyGain * quantity * productionMultiplier) / 100);
    }

    private int ReturnIdleGain()
    {
        idleGain = currencyGain * quantity + ((currencyGain * quantity * productionMultiplier) / 100);
        return idleGain;
    }

}
