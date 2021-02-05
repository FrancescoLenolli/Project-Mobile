using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Ship : MonoBehaviour
{
    private double totalCurrencyGain;

    public double currencyGain;
    public int quantity;
    public List<float> upgradeValues;

    public TEST_Ship(double currencyGain, int quantity, List<float> upgradeValues)
    {
        this.currencyGain = currencyGain;
        this.quantity = quantity;
        this.upgradeValues = upgradeValues;
    }

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
        float totalUpgrades = 0f;
        foreach(float upgrade in upgradeValues)
        {
            totalUpgrades += upgrade;
        }

        double newCurrencyGain = totalUpgrades == 0f ? currencyGain : currencyGain + (currencyGain * totalUpgrades / 100);

        totalCurrencyGain = newCurrencyGain * quantity;

        //Debug.Log($"{name} currencyGain\n\n" +
        //    $"CurrencyGain: {currencyGain}\nUpgrade percentage: {totalUpgrades}\n" +
        //    $"New CurrencyGain: {currencyGain} + {currencyGain * totalUpgrades / 100} = {currencyGain + (currencyGain * totalUpgrades / 100)}\n" +
        //    $"TotalCurrencyGain: {newCurrencyGain} * {quantity} = {newCurrencyGain * quantity}\n");
    }
}
