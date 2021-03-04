using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRPremiumCurrency : DailyReward
{
    public override void GetReward()
    {
        CurrencyManager.Instance.AddPremiumCurrencyFixedValue();
        Debug.Log("PremiumCurrencyReward");
    }

    public override Sprite GetSprite()
    {
        return CurrencyManager.Instance.data.premiumCurrencySprite;
    }
}
