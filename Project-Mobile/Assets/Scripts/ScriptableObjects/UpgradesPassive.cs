using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Upgrades", fileName = "PassiveUpgrade")]
public class UpgradesPassive : ScriptableObject
{
    // What ship does this Upgrade affects? 
    public ShipData.ShipType shipType = ShipData.ShipType.Patrol;
    public int index = 0;
    public string upgradeName = "";
    [TextArea(2, 10)]
    public string description = "";
    public Sprite upgradeSprite = null;
    public int cost = 0;

    // By how much the currencyGain is increased in percentages.
    public int multiplier = 0;
}
