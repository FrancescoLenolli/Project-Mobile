using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    protected double totalCurrencyGain;
    protected double cost;
    protected int quantity;

    public int GetQuantity()
    {
        return quantity;
    }

    public double GetTotalCurrencyGain()
    {
        return totalCurrencyGain;
    }

    public virtual void Buy()
    {
        // what happens when buying a unit of this collectible.
    }

    protected virtual void SetCost()
    {

    }

    protected virtual double GetUnitCurrencyGain()
    {
        return double.MaxValue;
        // calculate how much currency is gained by one unit of this collectible.
    }

    protected virtual void SetTotalCurrencyGain()
    {
        // calculate the total currency gained by the owned quantity of this collectible.
    }

    protected bool CanBuy()
    {
        return CurrencyManager.Instance.currency >= cost;
    }
}
