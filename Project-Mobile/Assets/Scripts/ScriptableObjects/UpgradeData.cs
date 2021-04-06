using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Upgrade", fileName = "New Upgrade")]
[System.Serializable]
public class UpgradeData : CollectibleData
{
    [Tooltip("Bonus percentage to currency gain of the Collectible that owns this upgrade.")]
    public float upgradePercentage;
}
