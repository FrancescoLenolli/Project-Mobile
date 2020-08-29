using System.Collections.Generic;
using UnityEngine;

public struct ShipInfo
{
    public ShipData shipData;
    public int shipQuantity;

    public ShipInfo(ShipData data, int quantity)
    {
        this.shipData = data;
        this.shipQuantity = quantity;
    }
}
public class ShipsManager : MonoBehaviour
{
    List<ShipData> listShipDatas = new List<ShipData>();
    Dictionary<int, ShipInfo> dctnShipsInfo = new Dictionary<int, ShipInfo>();

    public Ship prefabShip = null;
    public Transform containerShips = null;

    private void Awake()
    {
        listShipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));
    }

    private void Start()
    {
        InstantiateShips();
    }

    private void InstantiateShips()
    {
        // Handles first time the game is played.
        if(dctnShipsInfo.Count == 0)
        {
            dctnShipsInfo.Add(0, new ShipInfo(listShipDatas[0], 0));
        }

        // Spawn all ships currently unlocked.
        for(int i = 0; i < dctnShipsInfo.Count; ++i)
        {
            ShipData newData = dctnShipsInfo[i].shipData;
            int newQuantity = dctnShipsInfo[i].shipQuantity;

            Ship newShip = Instantiate(prefabShip, containerShips, false);
            newShip.SetValues(newData, newQuantity);
        }
    }

    public void AddShip(int newIndex, ShipData newData)
    {
        dctnShipsInfo.Add(newIndex, new ShipInfo(newData, 0));
    }
}
