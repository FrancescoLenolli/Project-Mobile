using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Ship : MonoBehaviour
{
    private double totalCurrencyGain;
    private int quantity = 10;

    [SerializeField] private TextMeshProUGUI textShipName = null;
    [SerializeField] private TextMeshProUGUI textShipCost = null;
    [SerializeField] private TextMeshProUGUI textShipCurrencyGain = null;
    [SerializeField] private TextMeshProUGUI textShipQuantity = null;
    [SerializeField] private Image imageShipIcon = null;
    [Space]
    public TEST_ShipData shipData;

    public void InitData(TEST_ShipData data)
    {
        shipData = data;
        SetTotalCurrencyGain();

        textShipName.text = data.name;
        textShipCost.text = Formatter.FormatValue(data.cost);
        textShipCurrencyGain.text = $"+ {Formatter.FormatValue(data.currencyGain)}/s";
        imageShipIcon.sprite = data.icon;
        SetTextQuantity();
    }

    public void Buy()
    {
        ++quantity;
        SetTotalCurrencyGain();
        SetTextQuantity();
    }

    public double GetTotalCurrencyGain()
    {
        return totalCurrencyGain;
    }

    private void SetTextQuantity()
    {
        textShipQuantity.text = $"x{quantity}";
    }

    private void SetTotalCurrencyGain()
    {
        double currencyGain = shipData.currencyGain;
        List<Upgrade> upgrades = shipData.upgrades;
        float totalUpgrades = 0f;

        foreach(Upgrade upgrade in upgrades.Where(x => x.isOwned))
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
