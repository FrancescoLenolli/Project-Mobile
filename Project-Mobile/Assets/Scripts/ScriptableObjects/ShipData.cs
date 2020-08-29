using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "CustomData/ShipData", fileName = "New Ship")]
public class ShipData : ScriptableObject
{
    public int index = 0;
    public string shipName = "New Ship";
    public Sprite shipIcon = null;
    public int cost = 0;
    public int currencyGain = 0;
}
