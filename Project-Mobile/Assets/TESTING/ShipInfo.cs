using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShipInfo
{
    public ShipData data;
    public int quantity;

    public ShipInfo(ShipData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;
    }
}
