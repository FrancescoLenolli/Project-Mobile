using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelShipsUpgrades : MonoBehaviour
{
    private List<ShipUpgradesData> listShipUpgrades = new List<ShipUpgradesData>();

    private void Awake()
    {
        listShipUpgrades = new List<ShipUpgradesData>(Resources.LoadAll<ShipUpgradesData>("Upgrades"));
    }

    public void UnlockUpgrades(int shipIndex)
    {
        //List<ShipUpgradesData> listUpgradesToSpawn = new List<ShipUpgradesData>();

        //foreach (ShipUpgradesData upgradesData in listShipUpgrades.Where(x => x.index == shipIndex))
        //{
        //    listUpgradesToSpawn.Add(upgradesData);
        //}

        //int listCount = listShipUpgrades.Count;
        //for(int i = 0; i < listCount; ++i)
        //{
        //    if
        //}
    }
}
