using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    private CanvasBottom canvasBottom;
    private List<ShipData> ownedShips = new List<ShipData>();
    private List<ShipData> shipsData = new List<ShipData>();

    private void Start()
    {
        shipsData = Resources.LoadAll<ShipData>("Ships").ToList();
        canvasBottom = FindObjectOfType<CanvasBottom>();
        InitShips();
    }

    private void InitShips()
    {
        if(ownedShips.Count == 0)
        {
            ownedShips.Add(shipsData[0]);
        }

        canvasBottom.InitData(ownedShips, this); //TODO: Create struct ShipInfo to store ShipData and Quantity.

    }

    public void UnlockNewShip(Ship ship)
    {
        int index = shipsData.IndexOf(ship.GetData()) + 1;

        if (index < shipsData.Count)
            canvasBottom.SpawnShip(shipsData[index], this);
    }
}
