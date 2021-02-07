using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasBottom : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private UIManager uiManager;

    public List<TEST_ShipData> shipDatas = new List<TEST_ShipData>();
    public Ship prefabShip;
    public Transform containerShips;

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;
        InitShip();
    }

    private void InitShip()
    {
        foreach(TEST_ShipData data in shipDatas)
        {
            Ship ship = Instantiate(prefabShip, containerShips, false);
            ship.InitData(data);
            currencyManager.AddShip(ship);
            uiManager.ResizeContainer(ship.transform, containerShips, UIManager.Resize.Add);
        }
    }
}
