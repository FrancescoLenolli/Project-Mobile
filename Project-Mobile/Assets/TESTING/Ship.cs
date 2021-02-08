using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class Ship : MonoBehaviour
{
    private Action<Ship> EventSendData;

    private ShipsManager shipsManager;
    private double totalCurrencyGain;
    private int quantity;
    private bool isOwned = false;

    [SerializeField] private TextMeshProUGUI textShipName = null;
    [SerializeField] private TextMeshProUGUI textShipCost = null;
    [SerializeField] private TextMeshProUGUI textShipCurrencyGain = null;
    [SerializeField] private TextMeshProUGUI textShipQuantity = null;
    [SerializeField] private Image imageShipIcon = null;
    [Space]
    public ShipData shipData;

    public void InitData(ShipData data, ShipsManager shipsManager)
    {
        if (data)
        {
            shipData = data;
            SetTotalCurrencyGain();

            textShipName.text = data.name;
            textShipCost.text = Formatter.FormatValue(data.cost);
            textShipCurrencyGain.text = $"+ {Formatter.FormatValue(data.currencyGain)}/s";
            imageShipIcon.sprite = data.icon;

            // Get quantity from saved data.
            SetTextQuantity();

            if (!shipData.IsQuantityEnough(quantity))
            {
                isOwned = false;
                this.shipsManager = shipsManager;
            }
            if(!isOwned && shipsManager)
            {
                SubscribeToEventSendData(shipsManager.UnlockNewShip);
            }
        }
    }

    public void Buy()
    {
        ++quantity;

        if (!isOwned && shipData.IsQuantityEnough(quantity))
        {
            EventSendData?.Invoke(this);
            UnsubscribeToEventSendData(shipsManager.UnlockNewShip);
            isOwned = true;
        }

        SetTotalCurrencyGain();
        SetTextQuantity();
    }

    public double GetTotalCurrencyGain()
    {
        return totalCurrencyGain;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public ShipData GetData()
    {
        return shipData;
    }

    private void SubscribeToEventSendData(Action<Ship> method)
    {
        EventSendData += method;
    }

    private void UnsubscribeToEventSendData(Action<Ship> method)
    {
        EventSendData -= method;
    }

    private void SetTextQuantity()
    {
        textShipQuantity.text = $"x{quantity}";
    }

    private void SetTotalCurrencyGain()
    {
        double currencyGain = shipData.currencyGain;
        List<UpgradeData> upgrades = shipData.upgrades;
        float totalUpgrades = 0f;

        foreach(UpgradeData upgrade in upgrades.Where(x => x.isOwned))
        {
            totalUpgrades += upgrade.upgradePercentage;
        }

        double newCurrencyGain = totalUpgrades == 0f ? currencyGain : currencyGain + (currencyGain * totalUpgrades / 100);

        totalCurrencyGain = newCurrencyGain * quantity;

        //Debug.Log($"{name} currencyGain\n\n" +
        //    $"CurrencyGain: {currencyGain}\nUpgrade percentage: {totalUpgrades}\n" +
        //    $"New CurrencyGain: {currencyGain} + {currencyGain * totalUpgrades / 100} = {currencyGain + (currencyGain * totalUpgrades / 100)}\n" +
        //    $"TotalCurrencyGain: {newCurrencyGain} * {quantity} = {newCurrencyGain * quantity}\n");
    }
}
