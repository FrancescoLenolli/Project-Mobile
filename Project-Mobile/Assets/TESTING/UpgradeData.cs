using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Upgrade", fileName = "New Upgrade")]
[System.Serializable]
public class UpgradeData : ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public Sprite icon;

    [Space]
    public double cost;
    public float upgradePercentage;
    public bool isOwned;

}
