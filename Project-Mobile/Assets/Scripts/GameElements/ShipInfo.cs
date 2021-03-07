using System.Collections.Generic;

[System.Serializable]
public struct ShipInfo
{
    public int index;
    public ShipData shipData;
    public int quantity;
    public List<UpgradeInfo> upgradesInfo;

    public ShipInfo(int index, ShipData shipData, int quantity, List<UpgradeInfo> upgradesInfo)
    {
        this.index = index;
        this.shipData = shipData;
        this.quantity = quantity;
        this.upgradesInfo = upgradesInfo;
    }

    public void SetData(ShipData shipData, List<UpgradeInfo> upgradesInfo)
    {
        this.shipData = shipData;
        this.upgradesInfo = upgradesInfo;
    }
}
