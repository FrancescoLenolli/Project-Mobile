using UnityEngine;

public class DRCurrency : DailyReward
{
    [Tooltip("Currency Gained when collecting this reward.")]
    public double currencyBonus = 0;

    public override void GetReward()
    {
        CurrencyManager.Instance.AddCurrency(currencyBonus);
    }
}
