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
    private RectTransform panelShipRect = null;

    public ShipUpgrade prefabShipUpgrade = null;
    public Transform panelShipUpgrades = null;

    public void InitUpgrades()
    {
        gameManager = GameManager.Instance;
        panelShipRect = panelShipUpgrades.GetComponent<RectTransform>();

        listUpgradesUnlocked = gameManager.playerData.playerUpgradesUnlocked;
        if (listUpgradesUnlocked == null)
        {
            listUpgradesUnlocked = new List<UpgradeInfo>();
        }

        List<ShipUpgradeData> listUpgradesOwned = new List<ShipUpgradeData>();
        List<ShipUpgradeData> listUpgradesNotOwned = new List<ShipUpgradeData>();

        // Put upgrades owned in a List separated from upgrades not owned.
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

        // Instantiate every upgrade not yet owned and resize their container.
        foreach (ShipUpgradeData upgradeData in listUpgradesNotOwned)
        {
            ShipUpgrade newUpgrade = Instantiate(prefabShipUpgrade, panelShipUpgrades, false);
            newUpgrade.SetValues(upgradeData, this);

            panelShipRect.sizeDelta = UIManager.Instance.ResizeContainer(panelShipUpgrades, newUpgrade.transform);
            newUpgrade.transform.SetSiblingIndex(0);

        }

        // TODO: Do something with Upgrades Owned.
    }

    // When the Player has enough units of a given Ship, unlock all of its Upgrades.
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

            // ...Resize container...
            panelShipRect.sizeDelta = UIManager.Instance.ResizeContainer(panelShipUpgrades, newUpgrade.transform);
            newUpgrade.transform.SetSiblingIndex(0);

            // ...Save the list of upgrades.
            gameManager.SaveUpgradesUnlocked(listUpgradesUnlocked);
        }
    }

    /// <summary>
    /// Set Upgrade status to Owned. Add it to the List of Owned Upgrades.
    /// </summary>
    /// <param name="upgradeData"></param>
    public void SetOwnedStatus(ShipUpgradeData upgradeData)
    {
        // Search for the right Upgrade and set his status to Owned, saving the modified list.
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

    public void ResizeContainer(Transform transform)
    {
        panelShipRect.sizeDelta = UIManager.Instance.ResizeContainer(panelShipUpgrades, transform, 0, UIManager.Resize.Subtract);
    }
}
