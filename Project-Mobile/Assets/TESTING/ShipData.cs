using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Ship", fileName = "New Ship")]
[System.Serializable]
public class ShipData : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public GameObject model;
    [Space]
    public double currencyGain;
    public double cost;
    public float costIncreaseMultiplier;
    public int qtForNextShip; //TODO: Fix horrendous name.
    public List<UpgradeData> upgrades;

    public bool IsQuantityEnough(int quantity)
    {
        return quantity >= qtForNextShip;
    }
}
