using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Ship", fileName = "New Ship")]
[System.Serializable]
public class ShipData : CollectibleData
{
    public GameObject model;
    [Space]
    [Tooltip("How much quantity of this Collectible is needed to unlock the next one.")]
    public int qtForNextShip; //TODO: Fix horrendous name.
    public List<UpgradeData> upgrades;
}
