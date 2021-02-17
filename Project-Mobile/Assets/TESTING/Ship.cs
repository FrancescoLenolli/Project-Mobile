using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private Action<ShipData> EventUnlockNewShip;

    private ShipsManager shipsManager;
    private double totalCurrencyGain;
    private double cost;
    private int quantity;
    private bool isNextShipUnlocked;
    public List<UpgradeInfo> upgradesInfo = new List<UpgradeInfo>();

    [SerializeField] private TextMeshProUGUI textShipName = null;
    [SerializeField] private TextMeshProUGUI textShipCost = null;
    [SerializeField] private TextMeshProUGUI textShipTotalCurrencyGain = null;
    [SerializeField] private TextMeshProUGUI textShipUnitCurrencyGain = null;
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
            gameObject.name = shipData.name;
            SetUpgradesInfo(shipInfo.upgradesInfo);
            SetCost();
            SetTotalCurrencyGain();

            textShipName.text = shipData.name;
            textShipUnitCurrencyGain.text = $"+ {Formatter.FormatValue(GetUnitCurrencyGain())}/s";
            textShipTotalCurrencyGain.text = $"+ {Formatter.FormatValue(GetTotalCurrencyGain())}/s";
            imageShipIcon.sprite = shipData.icon;

            SetTextQuantity();

            // No need to subscribe to EventUnlockNewShip if new ship is already unlocked.
            if (!IsQuantityEnough())
            {
                isNextShipUnlocked = false;
                this.shipsManager = shipsManager;
                SubscribeToEventSendData(shipsManager.UnlockNewShip);
            }
            else
            {
                isNextShipUnlocked = true;
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

            if (!isNextShipUnlocked && IsQuantityEnough())
            {
                EventUnlockNewShip?.Invoke(shipData);
                UnsubscribeToEventSendData(shipsManager.UnlockNewShip);
                isNextShipUnlocked = true;
            }

            SetTotalCurrencyGain();
            SetTextQuantity();

            if (GameManager.Instance.isVibrationOn)
                Vibration.VibrateSoft();
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
        for (int i = 0; i < upgradesInfo.Count; ++i)
        {
            if (upgradesInfo[i].upgradeData == upgradeData)
            {
                upgradesInfo[i] = new UpgradeInfo(upgradesInfo[i].upgradeData, true);
                SetTotalCurrencyGain();
                return;
            }
        }
    }

    private void SubscribeToEventSendData(Action<ShipData> method)
    {
        EventUnlockNewShip += method;
    }

    private void UnsubscribeToEventSendData(Action<ShipData> method)
    {
        EventUnlockNewShip -= method;
    }

    private bool IsQuantityEnough()
    {
        return quantity >= shipData.qtForNextShip;
    }

    private void SetTotalCurrencyGain()
    {
        double unitCurrencyGain = GetUnitCurrencyGain();
        totalCurrencyGain = unitCurrencyGain * quantity;

        textShipUnitCurrencyGain.text = $"+ {Formatter.FormatValue(GetUnitCurrencyGain())}/s";
        textShipTotalCurrencyGain.text = $"+ {Formatter.FormatValue(GetTotalCurrencyGain())}/s";
    }

    private double GetUnitCurrencyGain()
    {
        double currencyGain = shipData.currencyGain;
        List<UpgradeData> upgrades = shipData.upgrades;
        float totalUpgradesPct = 0f;

        foreach (UpgradeInfo info in upgradesInfo.Where(x => x.isOwned))
        {
            totalUpgradesPct += info.upgradeData.upgradePercentage;
        }

        double newCurrencyGain = totalUpgradesPct == 0f ? currencyGain : currencyGain + MathUtils.Pct(totalUpgradesPct, currencyGain);

        return newCurrencyGain;
    }

    private void SetUpgradesInfo(List<UpgradeInfo> upgradesInfo)
    {
        if (upgradesInfo.Count == 0)
        {
            foreach (UpgradeData data in shipData.upgrades)
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
