using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/TEST_Upgrade", fileName = "New Upgrade")]
public class Upgrade : ScriptableObject
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
