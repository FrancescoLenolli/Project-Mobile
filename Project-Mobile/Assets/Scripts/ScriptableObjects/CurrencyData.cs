using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/CurrencyData", fileName = "New CurrencyData")]
public class CurrencyData : ScriptableObject
{
    public Sprite currencySprite;
    public Sprite premiumCurrencySprite;
    public Sprite doubleGainTimeSprite;
    public double baseActiveGainValue;
    public int baseActiveGainPercentage;
    public int extrasPremiumCost;
    public int adPctGain;
    public int adHoursDoubleGain;
    public int adPremiumCurrencyGain;
}
