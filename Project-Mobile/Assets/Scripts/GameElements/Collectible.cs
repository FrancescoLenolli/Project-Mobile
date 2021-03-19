using UnityEngine;

public class Collectible : MonoBehaviour
{
    protected double baseCost;
    protected double baseCurrencyGain;
    protected double cost;
    private double totalCurrencyGain;
    private int quantity;
    private int weight;

    public int Quantity { get => quantity; set => quantity = value; }
    public int Weight { get => weight; set => weight = value; }
    public double TotalCurrencyGain { get => totalCurrencyGain; set => totalCurrencyGain = value; }
    public double BaseCost { get => baseCost; }

    public virtual void Buy()
    {
        // what happens when buying a unit of this collectible.
    }

    protected virtual void SetBaseCost()
    {
        //
    }

    protected virtual void SetCost()
    {
        //
    }

    protected virtual double GetUnitCurrencyGain()
    {
        // calculate how much currency is gained by one unit of this collectible.
        return double.MaxValue;
    }

    protected virtual void SetBaseCurrencyGain()
    {
        //
    }

    protected virtual void SetTotalCurrencyGain()
    {
        // calculate the total currency gained by the owned quantity of this collectible.
    }

    protected void SetWeight(int value)
    {
        weight = value;
    }

    protected bool CanBuy()
    {
        bool result = CurrencyManager.Instance.currency >= cost || GameManager.Instance.isTesting;
        return result;
    }
}
