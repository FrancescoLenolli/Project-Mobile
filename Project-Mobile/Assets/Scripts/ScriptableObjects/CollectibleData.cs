using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleData : ScriptableObject
{
    public int index;
    public new string name;
    public double currencyGain;
    public double cost;
    public float costIncreaseMultiplier;
}
