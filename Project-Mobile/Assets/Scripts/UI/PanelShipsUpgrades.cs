using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct UpgradeInfo
{
    // Generic Data for the Ship's Upgrade.
    // Things like cost, effects ecc...
    public ShipUpgradeData upgradeData;

    // Was this Upgrade bought or is still available to buy?
    // Upgrades NOT owned will be visible in the Upgrades panel.
    public bool isOwned;

    public UpgradeInfo(ShipUpgradeData newUpgradeData, bool newStatus)
    {
        upgradeData = newUpgradeData;
        isOwned = newStatus;
    }
}

public class PanelShipsUpgrades : MonoBehaviour
{
    private GameManager gameManager = null;
    private List<UpgradeInfo> listUpgradesUnlocked = new List<UpgradeInfo>();
    private List<UpgradeInfo> listUpgradesBought = new List<UpgradeInfo>();

    public ShipUpgrade prefabShipUpgrade = null;
    public Transform panelShipUpgrades = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        listUpgradesUnlocked = gameManager.playerData.playerUpgradesUnlocked;
    }

    public void InitUpgrades()
    {
        List<ShipUpgradeData> listUpgradesOwned = new List<ShipUpgradeData>();
        List<ShipUpgradeData> listUpgradesNotOwned = new List<ShipUpgradeData>();

        foreach (UpgradeInfo upgradeInfo in listUpgradesUnlocked)
        {
            if (upgradeInfo.isOwned)
            {
                listUpgradesOwned.Add(upgradeInfo.upgradeData);
            }
            else
            {
                listUpgradesNotOwned.Add(upgradeInfo.upgradeData);
            }
        }

        foreach (ShipUpgradeData upgradeData in listUpgradesNotOwned)
        {
            ShipUpgrade newUpgrade = Instantiate(prefabShipUpgrade, panelShipUpgrades, false);
            newUpgrade.SetValues(upgradeData, this);
        }
    }

    // When the Player has enough units of a Ship, unlock all of its Upgrades.
    public void UnlockUpgrades(ShipData.ShipType type)
    {
        List<ShipUpgradeData> listUpgrades = new List<ShipUpgradeData>(Resources.LoadAll<ShipUpgradeData>("Upgrades").Where(x => x.shipType == type));

        foreach (ShipUpgradeData shipUpgradeData in listUpgrades)
        {
            // Instantiate new Upgrade...
            ShipUpgrade newUpgrade = Instantiate(prefabShipUpgrade, panelShipUpgrades, false);
            newUpgrade.SetValues(shipUpgradeData, this);

            // ...Add it to the list of Upgrades Unlocked...
            UpgradeInfo upgradeUnlocked = new UpgradeInfo(shipUpgradeData, false);
            listUpgradesUnlocked.Add(upgradeUnlocked);

            // ...Save the list of upgrades.
            gameManager.SaveUpgradesUnlocked(listUpgradesUnlocked);
        }
    }

    // When buying an Upgrade, set his status to Owned so it won't be displayed in the Upgrades Panel when reloading the game.
    public void SetOwnedStatus(ShipUpgradeData upgradeData)
    {
        for (int i = 0; i < listUpgradesUnlocked.Count; ++i)
        {
            if (listUpgradesUnlocked[i].upgradeData == upgradeData)
            {
                listUpgradesUnlocked[i] = new UpgradeInfo(listUpgradesUnlocked[i].upgradeData, true);
                listUpgradesBought.Add(listUpgradesUnlocked[i]);

                gameManager.SaveUpgradesBought(listUpgradesBought);

                break;
            }
        }
    }
}
