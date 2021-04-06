using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/CurrencyData", fileName = "New CurrencyData")]
public class CurrencyData : ScriptableObject
{
    [Tooltip("Sprite representing the currency.")]
    public Sprite currencySprite;
    [Tooltip("Sprite representing the premium currency.")]
    public Sprite premiumCurrencySprite;
    [Tooltip("Sprite representing the double gain time.")]
    public Sprite doubleGainTimeSprite;
    [Tooltip("Base value of currency gained when tapping the screen.")]
    public double baseActiveGainValue;
    public double baseActiveGainPercentage;
    public double basePassiveGainPercentage;
    [Tooltip("Base cost of a ship. Cost of first ship with 0 quantity.")]
    public double baseShipCost;
    [Tooltip("Base currency gain value of a ship. currency gain of first ship with 0 quantity.")]
    public double baseShipCurrencyGain;
    [Tooltip("Cost in premium currency of extras.")]
    public int extrasPremiumCost;
    [Tooltip("Percentage added to offline and passive currency gain for every prestige level.")]
    public int prestigeOfflineBonusPct;
    [Tooltip("Percentage of current currency gained when watching a specific Ad.")]
    public int adPctGain;
    [Tooltip("Hours of double currency gain added when watching a specific Ad.")]
    public int adHoursDoubleGain;
    [Tooltip("Amount of premium currency gained when watching a specific Ad.")]
    public int adPremiumCurrencyGain;
}
