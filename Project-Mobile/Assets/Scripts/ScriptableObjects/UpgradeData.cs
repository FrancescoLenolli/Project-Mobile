using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Upgrade", fileName = "New Upgrade")]
[System.Serializable]
public class UpgradeData : CollectibleData
{
    public float upgradePercentage;
}
