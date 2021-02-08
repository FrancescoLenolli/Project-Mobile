using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasBottom : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private UIManager uiManager;

    public Ship prefabShip;
    public Transform containerShips;

    public void InitData(List<ShipData> ships, ShipsManager shipsManager)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        List<ShipData> shipDatas = ships;

        foreach(ShipData data in shipDatas)
        {
            SpawnShip(data, shipsManager);
        }
    }

    public void SpawnShip(ShipData data, ShipsManager shipsManager)
    {
        Ship ship = Instantiate(prefabShip, containerShips, false);
        ship.InitData(data, shipsManager);
        currencyManager.AddShip(ship);
        uiManager.ResizeContainer(ship.transform, containerShips, UIManager.Resize.Add);
    }
}
