using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct UpgradeInfo
{
    /// Generic Data for the Ship.
    /// Things like Name, Sprite, Currency Gain ecc... are all contained here.
    public ShipUpgradeData upgradeData;

    /// How many ships of the same type the player has.
    public bool isOwned;

    public UpgradeInfo(ShipUpgradeData newUpgradeData, bool newStatus)
    {
        upgradeData = newUpgradeData;
        isOwned = newStatus;
    }
}

public class PanelShipsUpgrades : MonoBehaviour
{
    private List<ShipUpgradeData> listShipUpgrades = new List<ShipUpgradeData>();

    public ShipUpgrade prefabShipUpgrade = null;
    public Transform panelShipUpgrades = null;


    private void Awake()
    {
        //listShipUpgrades = new List<ShipUpgradeData>(Resources.LoadAll<ShipUpgradeData>("Upgrades"));
    }

    public void UnlockUpgrades(ShipData.ShipType type)
    {
        List<ShipUpgradeData> listUpgrades = new List<ShipUpgradeData>(Resources.LoadAll<ShipUpgradeData>("Upgrades").Where(x => x.shipType == type));

        foreach(ShipUpgradeData shipUpgradeData in listUpgrades)
        {
            ShipUpgrade newUpgrade = Instantiate(prefabShipUpgrade, panelShipUpgrades, false);
            newUpgrade.SetValues(shipUpgradeData);
        }
    }
}
