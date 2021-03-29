using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private UpgradeData upgradeData;
    private Ship ship;
    private Transform container;
    private double cost;

    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textDescription = null;
    public TextMeshProUGUI textCost = null;
    public Image imageIcon = null;
    public Button buttonBuy = null;

    public void InitData(UpgradeData upgradeData, Ship ship, Transform container)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        this.upgradeData = upgradeData;
        this.container = container;
        this.ship = ship;
        SetCost(ship);

        textName.text = upgradeData.name;
        textDescription.text = $"Increase {ship.name} currency gain by {upgradeData.upgradePercentage}%";
        textCost.text = Formatter.FormatValue(cost);
        imageIcon.sprite = upgradeData.icon;

        Observer.AddObserver(ref currencyManager.EventSendCurrencyValue, SetButtonBuyStatus);
    }

    public void Buy()
    {
        if (CanBuy() || GameManager.Instance.isTesting)
        {
            if (!GameManager.Instance.isTesting)
                currencyManager.RemoveCurrency(cost);

            ship.UpgradeBought(upgradeData);
            uiManager.ResizeContainer(transform, container, UIManager.Resize.Subtract);

            Vibration.VibrateSoft();

            Observer.RemoveObserver(ref currencyManager.EventSendCurrencyValue, SetButtonBuyStatus);
            Destroy(gameObject);
        }
    }

    private bool CanBuy()
    {
        return cost <= currencyManager.currency;
    }

    private void SetCost(Ship ship)
    {
        double multiplier = Math.Pow(ship.shipData.index + 1, ship.shipData.index + 1) * 3;
        cost = ship.BaseCost * multiplier;
    }

    private void SetButtonBuyStatus(double totalCurrency)
    {
        if(buttonBuy)
        buttonBuy.interactable = cost <= totalCurrency;
    }
}
