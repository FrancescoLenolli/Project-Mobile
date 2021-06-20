using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveGainCalculator : IPassiveGainHandler
{
    public double GetTotalPassiveGain(CurrencyManager currencyManager)
    {
        double collectiblesGain = currencyManager.Collectibles.Sum(x => x.TotalCurrencyGain);
        double prestigeBonusGainMultiplier = 1 + (PrestigeManager.prestigeLevel / 10f);
        double totalGain = collectiblesGain * prestigeBonusGainMultiplier;

        if (currencyManager.SecondsDoubleGain > 0)
            totalGain *= 2;

        return totalGain;
    }
}
