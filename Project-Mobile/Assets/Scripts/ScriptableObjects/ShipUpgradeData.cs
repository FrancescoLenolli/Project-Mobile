using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Upgrades", fileName = "Upgrade")]
public class ShipUpgradeData : ScriptableObject
{
    // What ship does this Upgrade affects? 
    public ShipData.ShipType shipType = ShipData.ShipType.Patrol;
    public int index = 0;
    public string upgradeName = "";
    [TextArea(2, 10)]
    public string description = "";
    public Sprite upgradeSprite = null;
    public double cost = 0;

    // By how much the currencyGain is increased in percentages.
    // Example: productionMultiplier = 15, currencyGain is increased by 15%.
    // If I have two 15% upgrades, currencyGain is increased by 30%.
    public int productionMultiplier = 10;
}
