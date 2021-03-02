using UnityEngine;

public class Collectible : MonoBehaviour
{
    protected double cost;
    private double totalCurrencyGain;
    private int quantity;

    public int Quantity { get => quantity; set => quantity = value; }
    public double TotalCurrencyGain { get => totalCurrencyGain; set => totalCurrencyGain = value; }

    public virtual void Buy()
    {
        // what happens when buying a unit of this collectible.
    }

    protected virtual void SetCost()
    {
        // self explanatory.
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
        bool result = CurrencyManager.Instance.currency >= cost || GameManager.Instance.isTesting;
        return result;
    }
}
