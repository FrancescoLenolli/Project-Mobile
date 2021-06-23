using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private Action<CollectibleData> EventShowInfo;

    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private UpgradeData upgradeData;
    private Ship ship;
    private Transform container;
    private double cost;
    private PanelShipInfo panelShipInfo;

    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textEffect = null;
    public TextMeshProUGUI textCost = null;
    public Image imageIcon = null;
    public Button buttonBuy = null;

    public void InitData(UpgradeData upgradeData, Ship ship, Transform container, PanelShipInfo panelShipInfo)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        this.upgradeData = upgradeData;
        this.container = container;
        this.ship = ship;
        this.panelShipInfo = panelShipInfo;
        SetCost(ship);

        textName.text = upgradeData.name;
        textEffect.text = $"Increase {ship.name} currency gain by {upgradeData.upgradePercentage}%";
        textCost.text = Formatter.FormatValue(cost);
        imageIcon.sprite = upgradeData.icon;

        Observer.AddObserver(ref currencyManager.EventSendCurrencyValue, SetButtonBuyStatus);
        Observer.AddObserver(ref EventShowInfo, panelShipInfo.ShowInfo);
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
            Observer.RemoveObserver(ref EventShowInfo, panelShipInfo.ShowInfo);
            Destroy(gameObject);
        }
    }

    public void ShowInfo()
    {
        EventShowInfo?.Invoke(upgradeData);
    }

    private bool CanBuy()
    {
        return cost <= currencyManager.currency;
    }

    private void SetCost(Ship ship)
    {
        double multiplier = Math.Pow(ship.ShipData.index + 1, ship.ShipData.index + 1) * 3;
        cost = ship.BaseCost * multiplier;
    }

    private void SetButtonBuyStatus(double totalCurrency)
    {
        if(buttonBuy)
        buttonBuy.interactable = cost <= totalCurrency;
    }
}
