using UnityEngine;

public class DRCurrency : DailyReward
{
    public override void GetReward()
    {
        CurrencyManager.Instance.AddCurrencyFixedValue();
        Debug.Log("CurrencyReward");
    }

    public override Sprite GetSprite()
    {
        return CurrencyManager.Instance.data.currencySprite;
    }
}
