using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Ship", fileName = "New Ship")]
[System.Serializable]
public class ShipData : CollectibleData
{
    public GameObject model;
    [Space]
    [Tooltip("How much quantity of this Collectible is needed to unlock the next one.")]
    public int qtForNextShip;
    public List<UpgradeData> upgrades;
}
