using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "CustomData/ShipData", fileName = "New Ship")]
public class ShipData : ScriptableObject
{
    public bool isAvailable = false;
    public Sprite shipIcon = null;
    public int cost = 0;
    public int currencyGain = 0;
    [HideInInspector] public int quantity = 0;

    
}
