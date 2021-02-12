[System.Serializable]
public struct UpgradeInfo
{
    public UpgradeData upgradeData;
    public bool isOwned;

    public UpgradeInfo(UpgradeData upgradeData, bool isOwned)
    {
        this.upgradeData = upgradeData;
        this.isOwned = isOwned;
    }
}
