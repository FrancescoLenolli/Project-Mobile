using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private Action<ShipData> EventSendData;

    private ShipsManager shipsManager;
    private double totalCurrencyGain;
    private double cost;
    private int quantity;
    private bool isOwned = false;

    [SerializeField] private TextMeshProUGUI textShipName = null;
    [SerializeField] private TextMeshProUGUI textShipCost = null;
    [SerializeField] private TextMeshProUGUI textShipCurrencyGain = null;
    [SerializeField] private TextMeshProUGUI textShipQuantity = null;
    [SerializeField] private Image imageShipIcon = null;

    [Space]
    public ShipData shipData;
    public List<UpgradeInfo> upgradesInfo = new List<UpgradeInfo>();

    public void InitData(ShipData data, ShipsManager shipsManager, int quantity)
    {
        if (data)
        {
            shipData = data;
            this.quantity = quantity;
            SetCost();
            SetTotalCurrencyGain();

            textShipName.text = data.name;
            textShipCurrencyGain.text = $"+ {Formatter.FormatValue(data.currencyGain)}/s";
            imageShipIcon.sprite = data.icon;

            // Get quantity from saved data.
            SetTextQuantity();

            if (!shipData.IsQuantityEnough(quantity))
            {
                isOwned = false;
                this.shipsManager = shipsManager;
            }
            if (!isOwned && shipsManager)
            {
                SubscribeToEventSendData(shipsManager.UnlockNewShip);
            }
        }
    }

    public void Buy()
    {
        if (CanBuy() || GameManager.Instance.isTesting)
        {
            if (!GameManager.Instance.isTesting)
                CurrencyManager.Instance.RemoveCurrency(cost);

            ++quantity;
            SetCost();

            if (!isOwned && shipData.IsQuantityEnough(quantity))
            {
                EventSendData?.Invoke(shipData);
                UnsubscribeToEventSendData(shipsManager.UnlockNewShip);
                isOwned = true;
            }

            SetTotalCurrencyGain();
            SetTextQuantity();
        }
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

    public void SetTotalCurrencyGain()
    {
        double currencyGain = shipData.currencyGain;
        List<UpgradeData> upgrades = shipData.upgrades;
        float totalUpgradesPct = 0f;

        foreach (UpgradeData upgrade in upgrades.Where(x => x.isOwned))
        {
            totalUpgradesPct += upgrade.upgradePercentage;
        }

        double newCurrencyGain = totalUpgradesPct == 0f ? currencyGain : currencyGain + Utility.Pct(totalUpgradesPct, currencyGain);

        totalCurrencyGain = newCurrencyGain * quantity;
    }

    public void SetUpgradeInfo(UpgradeData upgradeData)
    {
    }

    private void SubscribeToEventSendData(Action<ShipData> method)
    {
        EventSendData += method;
    }

    private void UnsubscribeToEventSendData(Action<ShipData> method)
    {
        EventSendData -= method;
    }

    private void SetTextQuantity()
    {
        textShipQuantity.text = $"x{quantity}";
    }

    private void SetTextCost()
    {
        textShipCost.text = Formatter.FormatValue(cost);
    }

    private void SetCost()
    {
        double multiplier = Math.Pow(shipData.costIncreaseMultiplier, quantity);
        cost = shipData.cost * multiplier;
        SetTextCost();
    }

    private bool CanBuy()
    {
        return cost <= CurrencyManager.Instance.currency;
    }
}
