using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRDoubleGainTime : DailyReward
{
    public override void GetReward()
    {
        CurrencyManager.Instance.AddDoubleGainTime();
        Debug.Log("DoubleGainTimeReward");
    }

    public override Sprite GetSprite()
    {
        return CurrencyManager.Instance.data.doubleGainTimeSprite;
    }
}
