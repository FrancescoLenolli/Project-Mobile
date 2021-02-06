using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/TEST_ShipData", fileName = "New Ship")]
public class TEST_ShipData : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public GameObject model;
    [Space]
    public double currencyGain;
    public List<float> upgrades; // TODO: Make it a List of Upgrade class
}
