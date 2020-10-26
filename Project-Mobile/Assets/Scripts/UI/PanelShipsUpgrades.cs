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
    private List<UpgradeInfo> listUpgradesUnlocked = new List<UpgradeInfo>();

    public ShipUpgrade prefabShipUpgrade = null;
    public Transform panelShipUpgrades = null;

    private void Start()
    {
        listUpgradesUnlocked = GameManager.Instance.playerData.playerUpgrades;
        //InitUpgrades();
    }

    public void InitUpgrades()
    {
        List<ShipUpgradeData> listUpgradesOwned = new List<ShipUpgradeData>();
        List<ShipUpgradeData> listUpgradesNotOwned = new List<ShipUpgradeData>();

        foreach(UpgradeInfo upgradeInfo in listUpgradesUnlocked)
        {
            if(upgradeInfo.isOwned)
            {
                listUpgradesOwned.Add(upgradeInfo.upgradeData);
            }
            else
            {
                listUpgradesNotOwned.Add(upgradeInfo.upgradeData);
            }
        }

        foreach(ShipUpgradeData upgradeData in listUpgradesNotOwned)
        {
            ShipUpgrade newUpgrade = Instantiate(prefabShipUpgrade, panelShipUpgrades, false);
            newUpgrade.SetValues(upgradeData, this);
        }
    }

    public void UnlockUpgrades(ShipData.ShipType type)
    {
        List<ShipUpgradeData> listUpgrades = new List<ShipUpgradeData>(Resources.LoadAll<ShipUpgradeData>("Upgrades").Where(x => x.shipType == type));

        foreach(ShipUpgradeData shipUpgradeData in listUpgrades)
        {
            ShipUpgrade newUpgrade = Instantiate(prefabShipUpgrade, panelShipUpgrades, false);
            newUpgrade.SetValues(shipUpgradeData, this);

            UpgradeInfo upgradeBought = new UpgradeInfo(shipUpgradeData, false);
            listUpgradesUnlocked.Add(upgradeBought);

            GameManager.Instance.SaveUpgradesUnlocked(listUpgradesUnlocked);
        }
    }

    public void SetOwnedStatus(ShipUpgradeData upgradeData)
    {
        for(int i = 0; i < listUpgradesUnlocked.Count; ++i)
        {
            if(listUpgradesUnlocked[i].upgradeData == upgradeData)
            {
                listUpgradesUnlocked[i] = new UpgradeInfo(listUpgradesUnlocked[i].upgradeData, true);
                break;
            }
        }
    }
}
