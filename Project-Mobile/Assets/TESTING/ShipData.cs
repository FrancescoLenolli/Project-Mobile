using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/ShipData", fileName = "New Ship")]
public class ShipData : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public GameObject model;
    [Space]
    public double currencyGain;
    public double cost;
    public List<Upgrade> upgrades; // TODO: Make it a List of Upgrade class
}
