using System.Collections.Generic;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    List<ShipData> shipDatas = null;
    //List<int> quantities = null; // How much of a shipData is owned 

    public Ship prefabShip = null;
    public Transform containerShips = null;

    private void Awake()
    {
        shipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));
    }

    private void Start()
    {
        InstantiateShips();
    }

    private void InstantiateShips()
    {
        foreach (ShipData ship in shipDatas)
        {
            if (ship.isAvailable)
            {
                Ship newShip = Instantiate(prefabShip, containerShips, false);
                newShip.SetValues(ship);
            }
            else break; // When we find a ship with isAvailable = false, all the ships after it are also unavailable, so we can stop the function here
        }
    }
}
