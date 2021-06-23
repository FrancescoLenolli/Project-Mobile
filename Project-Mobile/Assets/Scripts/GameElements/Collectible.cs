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
        //
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
        return double.MaxValue;
    }

    protected virtual void SetBaseCurrencyGain()
    {
        //
    }

    protected virtual void SetTotalCurrencyGain()
    {
        //
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
