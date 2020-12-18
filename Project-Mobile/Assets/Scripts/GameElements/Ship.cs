using System;
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
public delegate void ShowShip();
public delegate void UpdateIdleGain();
public delegate void UpdateCurrencyText(double value);
public delegate void ShowPanelDescription(Sprite sprite, string name, string description);

public class Ship : MonoBehaviour
{
    public event BoughtShip EventBoughtShip;
    public event UnlockShip EventUnlockShip;
    public event UpdateShipQuantity EventUpdateShipQuantity;
    public event UpdateShipModifier EventUpdateShipModifier;
    public event UnlockUpgrades EventUnlockUpgrades;
    public event ShowShip EventShowShip;
    public event UpdateIdleGain EventUpdateIdleGain;
    public event UpdateCurrencyText EventUpdateCurrencyText;
    public event ShowPanelDescription EventShowPanelDescriptions;

    private CurrencyManager currencyManager = null;
    private CanvasBottom canvasBottom = null;

    private ShipData.ShipType shipType = ShipData.ShipType.Patrol;
    private int index = 0;
    private string shipName = "";
    private string shipDescription = "";
    private Sprite shipIcon = null;
    private double cost = 0;
    private double currencyGain = 0;
    // How much currency does this ship gain every second.
    private double idleGain = 0;
    // CurrencyGain increased if more X units are bought.
    private double additionalCurrencyGain = 0;
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
    public List<Sprite> listButtonSprites = new List<Sprite>();

    private void Update()
    {

        // TODO: Add a sprite change.
        // Call this with an event. 
        // When idle currency is updated, when active currency is updated, when buying new ships.
        if (currencyManager || listButtonSprites != null)
        {
            bool canBuy = currencyManager.currency >= cost;

            buttonBuy.image.sprite = canBuy ? listButtonSprites[0] : listButtonSprites[1];
            buttonBuy.interactable = canBuy;
        }

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
        EventUnlockShip?.Invoke(index);
    }

    // Update Quantity of this Ship in ShipInfo, that will be saved.
    private void UpdateQuantity(int index, int quantity)
    {
        EventUpdateShipQuantity?.Invoke(index, quantity);
    }

    private void UpdateMultiplier(int index, int multiplier)
    {
        EventUpdateShipModifier?.Invoke(index, multiplier);
    }

    // Recalculate the idleGain of this Ship;
    private void UpdateIdleGain()
    {
        idleGain = currencyGain * quantity + ((currencyGain * quantity * productionMultiplier) / 100);
    }

    private double ReturnIdleGain()
    {
        idleGain = currencyGain * quantity + ((currencyGain * quantity * productionMultiplier) / 100);
        return idleGain;
    }

    public void SetValues(ShipData newShipData, int newQuantity, int newMultiplier)
    {
        currencyManager = CurrencyManager.Instance;
        // TODO: Use a UI Manager to pass reference to canvasBottom?
        canvasBottom = FindObjectOfType<CanvasBottom>();

        canvasBottom.EventUpdateQuantityModifier += UpdateQuantityModifier;
        EventUnlockShip += canvasBottom.panelShips.UnlockNewShip;
        EventUpdateShipQuantity += canvasBottom.panelShips.UpdateQuantityAt;
        EventUpdateShipModifier += canvasBottom.panelShips.UpdateModifierAt;
        EventUnlockUpgrades += canvasBottom.panelShipsUpgrades.UnlockUpgrades;
        EventShowShip += FindObjectOfType<ShipsView>().ShowNewShip;
        EventUpdateIdleGain += UpdateIdleGain;
        EventUpdateCurrencyText += FindObjectOfType<CanvasMain>().UpdateCurrencyText;
        EventShowPanelDescriptions += FindObjectOfType<PanelItemDescription>().ShowPanel;

        quantity = newQuantity;
        productionMultiplier = newMultiplier;
        shipData = newShipData;
        shipType = shipData.shipType;
        index = shipData.index;
        shipName = shipData.shipName;
        shipDescription = shipData.shipDescription;
        shipIcon = shipData.shipIcon;
        cost = shipData.cost;
        currencyGain = shipData.currencyGain;
        qtToUnlockNextShip = shipData.qtToUnlockNextShip;

        quantityMultiplier = currencyManager.GetShipQuantityToBuy();
        cost *= quantityMultiplier;

        additionalCurrencyGain = currencyGain * quantityMultiplier;

        currencyManager.IncreaseCurrencyIdleGain(ReturnIdleGain());

        textName.text = shipName;
        imageIcon.sprite = shipIcon;
        textCost.text = Formatter.FormatValue(cost);
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = "+" + Formatter.FormatValue(ReturnIdleGain()) + "/s";
        textAdditionalCurrencyGain.text = "+" + Formatter.FormatValue(additionalCurrencyGain) + "/s";
    }

    // Update Values when the Player change the quantity of ships to buy.
    public void UpdateValues()
    {
        quantityMultiplier = currencyManager.GetShipQuantityToBuy();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        UpdateIdleGain();

        textCost.text = Formatter.FormatValue(cost);
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = "+" + Formatter.FormatValue(ReturnIdleGain()) + "/s";
        textAdditionalCurrencyGain.text = "+" + Formatter.FormatValue(additionalCurrencyGain) + "/s";
    }

    // Method called from UI button to buy units of a Ship.
    public void Buy()
    {
        int multiplier = quantityMultiplier;

        // Ship can be bought.
        if (currencyManager.currency >= cost)
        {
            currencyManager.currency -= cost;
            EventUpdateCurrencyText?.Invoke(currencyManager.currency);

            // If this is the first time buying this ship, unlock his upgrades.
            if(quantity == 0)
            {
                EventUnlockUpgrades?.Invoke(shipType);
                EventShowShip?.Invoke();
            }

            // Recalculate IdleGain according to new Quantity.
            UpdateIdleGainForQuantity(multiplier);
            UpdateValues();
            UpdateQuantity(index, quantity);

            // Notify UpgradesPanel.
            EventBoughtShip?.Invoke(shipName);

            if (quantity >= shipData.qtToUnlockNextShip) UnlockNextShip(index);

            // Play sound.
        }
        // Ship cannot be bought.
        else
        {
            // Play different sound.
        }
    }

    public void UpdateIdleGain(int multiplier)
    {
        currencyManager.DecreaseCurrencyIdleGain(idleGain);

        productionMultiplier += multiplier;
        EventUpdateIdleGain?.Invoke();

        currencyManager.IncreaseCurrencyIdleGain(idleGain);

        UpdateValues();
        UpdateMultiplier(shipData.index, productionMultiplier);
    }

    public void UpdateIdleGainForQuantity(int additionalQuantity)
    {
        currencyManager.DecreaseCurrencyIdleGain(idleGain);

        quantity += additionalQuantity;
        EventUpdateIdleGain?.Invoke();

        currencyManager.IncreaseCurrencyIdleGain(idleGain);
    }

    public void ShowPanelDescription()
    {
        EventShowPanelDescriptions?.Invoke(shipIcon, shipName, shipDescription);
    }
}
