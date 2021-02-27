using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : Collectible
{
    private Action<ShipData> EventUnlockNewShip;
    private Action<ShipData> EventSpawnShipModel;

    private ShipsManager shipsManager;
    private bool isNextShipUnlocked;
    private bool canAutoBuy;
    private bool isButtonHeld;
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

            canAutoBuy = false;
            StartCoroutine(AutoBuy());

            if(quantity > 0)
            {
                SubscribeToEventSpawnShip(shipsManager.SpawnShipModel);
                EventSpawnShipModel?.Invoke(shipData);
                UnsubscribeToEventSpawnShip(shipsManager.SpawnShipModel);
            }
            else
            {
                SubscribeToEventSpawnShip(shipsManager.SpawnShipModel);
            }    
        }
    }

    public override void Buy()
    {
        if (CanBuy() || GameManager.Instance.isTesting)
        {
            if (!GameManager.Instance.isTesting)
                CurrencyManager.Instance.RemoveCurrency(cost);

            ++quantity;
            if(quantity > 0 && !isNextShipUnlocked)
            {
                EventSpawnShipModel?.Invoke(shipData);
                UnsubscribeToEventSpawnShip(shipsManager.SpawnShipModel);
            }

            SetCost();
            SetTotalCurrencyGain();
            SetTextQuantity();

            if (!isNextShipUnlocked && IsQuantityEnough())
            {
                EventUnlockNewShip?.Invoke(shipData);
                UnsubscribeToEventSendData(shipsManager.UnlockNewShip);
                isNextShipUnlocked = true;
                canAutoBuy = false;
            }

            if (GameManager.Instance.isVibrationOn)
                Vibration.VibrateSoft();
        }
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
                upgradesInfo[i] = new UpgradeInfo(upgradesInfo[i].index, upgradesInfo[i].upgradeData, true);
                SetTotalCurrencyGain();
                return;
            }
        }
    }

    public void StartAutoBuy()
    {
        isButtonHeld = true;
        StartCoroutine(AutoBuyStartingDelay());
    }

    public void StopAutoBuy()
    {
        canAutoBuy = false;
        isButtonHeld = false;
    }


    private void SubscribeToEventSendData(Action<ShipData> method)
    {
        EventUnlockNewShip += method;
    }

    private void UnsubscribeToEventSendData(Action<ShipData> method)
    {
        EventUnlockNewShip -= method;
    }

    private void SubscribeToEventSpawnShip(Action<ShipData> method)
    {
        EventSpawnShipModel += method;
    }

    private void UnsubscribeToEventSpawnShip(Action<ShipData> method)
    {
        EventSpawnShipModel -= method;
    }


    private bool IsQuantityEnough()
    {
        return quantity >= shipData.qtForNextShip;
    }

    protected override double GetUnitCurrencyGain()
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

    protected override void SetTotalCurrencyGain()
    {
        double unitCurrencyGain = GetUnitCurrencyGain();
        totalCurrencyGain = unitCurrencyGain * quantity;

        textShipUnitCurrencyGain.text = $"+ {Formatter.FormatValue(GetUnitCurrencyGain())}/s";
        textShipTotalCurrencyGain.text = $"+ {Formatter.FormatValue(GetTotalCurrencyGain())}/s";
    }

    protected override void SetCost()
    {
        double multiplier = Math.Pow(shipData.costIncreaseMultiplier, quantity);
        cost = shipData.cost * multiplier;
        SetTextCost();
    }

    private void SetUpgradesInfo(List<UpgradeInfo> upgradesInfo)
    {
        if (upgradesInfo.Count == 0)
        {
            foreach (UpgradeData data in shipData.upgrades)
            {
                this.upgradesInfo.Add(new UpgradeInfo(data.index, data, false));
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

    private IEnumerator AutoBuy()
    {
        while(true)
        {
            if (canAutoBuy)
            {
                yield return new WaitForSeconds(.05f);
                Buy();
                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator AutoBuyStartingDelay()
    {
        yield return new WaitForSeconds(1.3f);

        if(isButtonHeld)
        canAutoBuy = true;

        yield return null;
    }
}
