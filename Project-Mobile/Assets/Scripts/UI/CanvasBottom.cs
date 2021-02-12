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

    public void InitData(List<ShipInfo> ships, ShipsManager shipsManager)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        List<ScrollRect> list = containersParent.GetComponentsInChildren<ScrollRect>().ToList();
        list.ForEach(x => containers.Add(x.transform));

        OpenPanel(0);

        List<ShipInfo> shipDatas = ships;
        foreach (ShipInfo ship in shipDatas)
        {
            SpawnShip(ship.data, shipsManager, ship.quantity);
        }
    }

    public void SpawnShip(ShipData data, ShipsManager shipsManager, int quantity = 0)
    {
        Ship ship = Instantiate(prefabShip, containerShips, false);
        ship.InitData(data, shipsManager, quantity);
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
        foreach (UpgradeData upgradeData in ship.GetData().upgrades.Where(x => !x.isOwned))
        {
            Upgrade upgrade = Instantiate(prefabUpgrade, containerUpgrades, false);
            upgrade.InitData(upgradeData, ship, containerUpgrades);

            upgrade.transform.SetAsFirstSibling();
            uiManager.ResizeContainer(upgrade.transform, containerUpgrades, UIManager.Resize.Add);
        }
    }
}
