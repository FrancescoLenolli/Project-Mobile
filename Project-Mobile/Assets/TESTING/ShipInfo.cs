using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShipInfo
{
    public ShipData data;
    public int quantity;
    public List<UpgradeInfo> upgradesInfo;

    public ShipInfo(ShipData data, int quantity, List<UpgradeInfo> upgradesInfo)
    {
        this.data = data;
        this.quantity = quantity;
        this.upgradesInfo = upgradesInfo;
    }
}
