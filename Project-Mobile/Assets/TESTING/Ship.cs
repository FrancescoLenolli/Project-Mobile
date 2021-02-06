using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private double totalCurrencyGain;
    private int quantity = 10;

    public TEST_ShipData shipData;

    private void Start()
    {
        SetTotalCurrencyGain();    
    }

    public void Buy()
    {
        ++quantity;
        SetTotalCurrencyGain();
    }

    public double GetTotalCurrencyGain()
    {
        return totalCurrencyGain;
    }

    private void SetTotalCurrencyGain()
    {
        double currencyGain = shipData.currencyGain;
        List<Upgrade> upgrades = shipData.upgrades;
        float totalUpgrades = 0f;

        foreach(Upgrade upgrade in upgrades)
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
