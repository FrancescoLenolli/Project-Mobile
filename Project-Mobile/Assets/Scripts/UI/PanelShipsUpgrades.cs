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
    private GameManager gameManager = null;
    private List<UpgradeInfo> listUpgradesUnlocked = new List<UpgradeInfo>();
    // Have a List in GameManager to save the bought upgrades, maybe add a panel to see all of them?
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
