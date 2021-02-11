using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    private Action<List<ShipInfo>, ShipsManager> EventSendData;
    private Action<ShipData, ShipsManager, int> EventUnlockShip;

    private CanvasBottom canvasBottom;
    private List<ShipInfo> ownedShips = new List<ShipInfo>();
    private List<ShipData> totalShips = new List<ShipData>();

    public void InitData()
    {
        ownedShips = SaveManager.GetData().ships;
        totalShips = Resources.LoadAll<ShipData>("Ships").ToList();
        canvasBottom = FindObjectOfType<CanvasBottom>();

        SubscribeToEventSendData(canvasBottom.InitData);
        SubscribeToEventUnlockShip(canvasBottom.SpawnShip);

        if (ownedShips.Count == 0)
        {
            ShipInfo firstShip = new ShipInfo(totalShips[0], 0);
            ownedShips.Add(firstShip);
        }

        EventSendData?.Invoke(ownedShips, this);

    }

    public void UnlockNewShip(ShipData shipData)
    {
        int index = totalShips.IndexOf(shipData) + 1;

        if (index < totalShips.Count)
            EventUnlockShip?.Invoke(totalShips[index], this, 0);
    }

    public void SaveData()
    {
        List<Ship> ships = canvasBottom.GetShips();
        List<ShipInfo> shipsInfo = new List<ShipInfo>();

        for(int i = 0; i < ships.Count; ++i)
        {
            ShipInfo shipInfo = new ShipInfo(ships[i].shipData, ships[i].GetQuantity());
            shipsInfo.Add(shipInfo);
        }

        SaveManager.GetData().ships = shipsInfo;
    }

    public void SubscribeToEventSendData(Action<List<ShipInfo>, ShipsManager> method)
    {
        EventSendData += method;
    }

    public void UnsubscribeToEventSendData(Action<List<ShipInfo>, ShipsManager> method)
    {
        EventSendData -= method;
    }

    public void SubscribeToEventUnlockShip(Action<ShipData, ShipsManager, int> method)
    {
        EventUnlockShip += method;
    }
}
