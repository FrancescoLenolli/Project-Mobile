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
    public List<UpgradeInfo> upgradesInfo = new List<UpgradeInfo>();

    [SerializeField] private TextMeshProUGUI textShipName = null;
    [SerializeField] private TextMeshProUGUI textShipCost = null;
    [SerializeField] private TextMeshProUGUI textShipCurrencyGain = null;
    [SerializeField] private TextMeshProUGUI textShipQuantity = null;
    [SerializeField] private Image imageShipIcon = null;

    [Space]
    public ShipData shipData;

    public void InitData(ShipInfo shipInfo, ShipsManager shipsManager)
    {
        if (shipInfo.data)
        {
            shipData = shipInfo.data;
            quantity = shipInfo.quantity;
            SetUpgradesInfo(shipInfo.upgradesInfo);
            SetCost();
            SetTotalCurrencyGain();

            textShipName.text = shipData.name;
            textShipCurrencyGain.text = $"+ {Formatter.FormatValue(shipData.currencyGain)}/s";
            imageShipIcon.sprite = shipData.icon;

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

    public List<UpgradeInfo> GetUpgradesInfo()
    {
        return upgradesInfo;
    }

    public void UpgradeBought(UpgradeData upgradeData)
    {
        for(int i = 0; i < upgradesInfo.Count; ++i)
        {
            if(upgradesInfo[i].upgradeData == upgradeData)
            {
                upgradesInfo[i] = new UpgradeInfo(upgradesInfo[i].upgradeData, true);
                SetTotalCurrencyGain();
                return;
            }
        }
    }

    private void SubscribeToEventSendData(Action<ShipData> method)
    {
        EventSendData += method;
    }

    private void UnsubscribeToEventSendData(Action<ShipData> method)
    {
        EventSendData -= method;
    }

    private void SetTotalCurrencyGain()
    {
        double currencyGain = shipData.currencyGain;
        List<UpgradeData> upgrades = shipData.upgrades;
        float totalUpgradesPct = 0f;

        foreach(UpgradeInfo info in upgradesInfo.Where(x => x.isOwned))
        {
            totalUpgradesPct += info.upgradeData.upgradePercentage;
        }

        double newCurrencyGain = totalUpgradesPct == 0f ? currencyGain : currencyGain + Utility.Pct(totalUpgradesPct, currencyGain);

        totalCurrencyGain = newCurrencyGain * quantity;
    }

    private void SetUpgradesInfo(List<UpgradeInfo> upgradesInfo)
    {
        if(upgradesInfo.Count == 0)
        {
            foreach(UpgradeData data in shipData.upgrades)
            {
                this.upgradesInfo.Add(new UpgradeInfo(data, false));
            }
        }
        else
        {
            this.upgradesInfo = upgradesInfo;
        }
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
