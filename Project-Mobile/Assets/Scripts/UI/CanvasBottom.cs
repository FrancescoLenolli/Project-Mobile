using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBottom : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private List<Transform> containers = new List<Transform>();
    private List<Ship> ships = new List<Ship>();

    public Ship prefabShip;
    public Upgrade prefabUpgrade;
    public Transform containersParent;
    public Transform containerShips;
    public Transform containerUpgrades;

    public void InitData(List<ShipInfo> shipsInfo, ShipsManager shipsManager)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        List<ScrollRect> list = containersParent.GetComponentsInChildren<ScrollRect>().ToList();
        list.ForEach(x => containers.Add(x.transform));

        OpenPanel(0);

        for (int i = 0; i < shipsInfo.Count; ++i)
        {
            SpawnShip(shipsInfo[i], shipsManager);

            if(i == shipsInfo.Count - 1 && shipsInfo[i].quantity > 0)
            {
                shipsManager.ViewShip();
            }
        }
    }

    public void SpawnShip(ShipInfo shipInfo, ShipsManager shipsManager)
    {
        Ship ship = Instantiate(prefabShip, containerShips, false);
        ship.InitData(shipInfo, shipsManager);
        SpawnUpgrades(ship);
        currencyManager.AddShip(ship);
        ships.Add(ship);

        ship.transform.SetAsFirstSibling();
        uiManager.ResizeContainer(ship.transform, containerShips, UIManager.Resize.Add);
    }

    public List<Ship> GetShips()
    {
        return ships;
    }

    public void OpenPanel(int index)
    {
        uiManager.ChangeVisibility(containers, index);
    }

    private void SpawnUpgrades(Ship ship)
    {
        foreach(UpgradeInfo info in ship.GetUpgradesInfo().Where(x => !x.isOwned))
        {
            Upgrade upgrade = Instantiate(prefabUpgrade, containerUpgrades, false);
            upgrade.InitData(info.upgradeData, ship, containerUpgrades);

            upgrade.transform.SetAsFirstSibling();
            uiManager.ResizeContainer(upgrade.transform, containerUpgrades, UIManager.Resize.Add);
        }
    }
}
