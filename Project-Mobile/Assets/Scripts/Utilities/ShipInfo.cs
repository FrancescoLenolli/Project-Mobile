using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShipInfo
{
    public int dataIndex;
    public ShipData data;
    public int quantity;
    public List<UpgradeInfo> upgradesInfo;

    public ShipInfo(int dataIndex, ShipData data, int quantity, List<UpgradeInfo> upgradesInfo)
    {
        this.dataIndex = dataIndex;
        this.data = data;
        this.quantity = quantity;
        this.upgradesInfo = upgradesInfo;
    }
}
