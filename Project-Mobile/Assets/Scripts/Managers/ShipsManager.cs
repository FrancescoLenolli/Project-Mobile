using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    private Action<List<ShipInfo>, ShipsManager> EventSendData;
    private Action<ShipInfo, ShipsManager> EventUnlockShip;

    private UIManager uiManager;
    private CanvasBottom canvasBottom;
    private List<ShipInfo> savedShipsInfo;
    private List<ShipData> totalShips = new List<ShipData>();
    private List<GameObject> shipsModel = new List<GameObject>();
    private Vector3 shipsStartingPosition = new Vector3(0f, 0f, -500f);
    private int currentModelIndex = 0;

    public Transform shipsParent;

    public void InitData()
    {
        savedShipsInfo = SaveManager.GetData().ships;
        totalShips = Resources.LoadAll<ShipData>("Ships").ToList();
        canvasBottom = FindObjectOfType<CanvasBottom>();
        uiManager = UIManager.Instance;

        SubscribeToEventSendData(canvasBottom.InitData);
        SubscribeToEventUnlockShip(canvasBottom.SpawnShip);

        if (savedShipsInfo.Count == 0)
        {
            ShipInfo firstShip = new ShipInfo(totalShips[0], 0, new List<UpgradeInfo>());

            savedShipsInfo.Add(firstShip);
        }

        EventSendData?.Invoke(savedShipsInfo, this);

    }

    public void UnlockNewShip(ShipData shipData)
    {
        int index = totalShips.IndexOf(shipData) + 1;

        if (index < totalShips.Count)
        {
            ShipInfo newShipInfo = new ShipInfo(totalShips[index], 0, new List<UpgradeInfo>());
            EventUnlockShip?.Invoke(newShipInfo, this);
        }
    }

    public void SpawnShipModel(ShipData shipData)
    {
        GameObject shipModel = Instantiate(shipData.model, shipsStartingPosition, Quaternion.identity);
        shipsModel.Add(shipModel);
        CycleModels(UIManager.Cycle.Right);
    }

    public void CycleModels(UIManager.Cycle cycleType)
    {
        if (shipsModel.Count > 0)
        {
            HideShip();

            currentModelIndex = uiManager.CycleListIndexOpen(currentModelIndex, shipsModel.Count, cycleType);

            ViewShip();
        }
    }

    public void HideShip()
    {
        shipsModel[currentModelIndex].transform.SetParent(null);
        shipsModel[currentModelIndex].transform.position = shipsStartingPosition;
    }

    public void ViewShip()
    {
        shipsModel[currentModelIndex].transform.SetParent(shipsParent);
        shipsModel[currentModelIndex].transform.localPosition = Vector3.zero;
        shipsModel[currentModelIndex].transform.rotation = shipsParent.rotation;
    }


    public void SaveData()
    {
        List<Ship> ships = canvasBottom.GetShips();
        List<ShipInfo> shipsInfo = new List<ShipInfo>();

        for(int i = 0; i < ships.Count; ++i)
        {
            ShipInfo shipInfo = new ShipInfo(ships[i].shipData, ships[i].GetQuantity(), ships[i].GetUpgradesInfo());
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

    public void SubscribeToEventUnlockShip(Action<ShipInfo, ShipsManager> method)
    {
        EventUnlockShip += method;
    }
}
