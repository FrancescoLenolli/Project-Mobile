using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRDoubleGainTime : DailyReward
{
    [Tooltip("Time in HOURS where idle currency gain is doubled.")]
    public int doubleGainTime = 0;
    public override void GetReward()
    {
        //TODO: DailyReward
        //CurrencyManager.Instance.AddDoubleIdleGainTime(doubleGainTime * 3600);
    }
}
