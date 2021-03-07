[System.Serializable]
public struct UpgradeInfo
{
    public int index;
    public UpgradeData upgradeData;
    public bool isOwned;

    public UpgradeInfo(int index, UpgradeData upgradeData, bool isOwned)
    {
        this.index = index;
        this.upgradeData = upgradeData;
        this.isOwned = isOwned;
    }

    public void SetData(UpgradeData upgradeData)
    {
        this.upgradeData = upgradeData;
    }
}
