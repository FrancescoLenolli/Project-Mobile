using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasBottom : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private List<Ship> ships = new List<Ship>();

    public Ship prefabShip;
    public Transform containerShips;

    public void InitData(List<ShipInfo> ships, ShipsManager shipsManager)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        List<ShipInfo> shipDatas = ships;

        foreach(ShipInfo ship in shipDatas)
        {
            SpawnShip(ship.data, shipsManager, ship.quantity);
        }
    }

    public void SpawnShip(ShipData data, ShipsManager shipsManager, int quantity = 0)
    {
        Ship ship = Instantiate(prefabShip, containerShips, false);
        ship.InitData(data, shipsManager, quantity);
        currencyManager.AddShip(ship);
        ships.Add(ship);
        uiManager.ResizeContainer(ship.transform, containerShips, UIManager.Resize.Add);
    }

    public List<Ship> GetShips()
    {
        return ships;
    }
}
