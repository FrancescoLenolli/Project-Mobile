using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    private Action<List<ShipInfo>, ShipsManager> EventSendData;
    private Action<ShipInfo, ShipsManager> EventUnlockShip;
    private Action<int> EventShipViewChanged;

    private UIManager uiManager;
    private CanvasBottom canvasBottom;
    private List<ShipInfo> savedShipsInfo;
    private List<ShipData> totalShips = new List<ShipData>();
    private List<UpgradeData> totalUpgrades = new List<UpgradeData>();
    private List<GameObject> shipsModel = new List<GameObject>();
    private Vector3 shipsStartingPosition = new Vector3(0f, 0f, -500f);
    private int currentModelIndex = 0;

    [SerializeField] private Transform shipsParent = null;

    public void InitData()
    {
        CameraBehaviour cameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
        uiManager = UIManager.Instance;
        savedShipsInfo = SaveManager.PlayerData.ships;
        totalShips = Resources.LoadAll<ShipData>("Ships").ToList();
        totalUpgrades = Resources.LoadAll<UpgradeData>("Upgrades").ToList();
        canvasBottom = FindObjectOfType<CanvasBottom>();

        Observer.AddObserver(ref EventSendData, canvasBottom.InitData);
        Observer.AddObserver(ref EventUnlockShip, canvasBottom.SpawnShip);
        Observer.AddObserver(ref EventShipViewChanged, cameraBehaviour.ChangePosition);

        // Handles first play and data resets.
        if (savedShipsInfo.Count == 0)
        {
            ShipInfo firstShip = new ShipInfo(totalShips[0].index, totalShips[0], 0, new List<UpgradeInfo>());

            savedShipsInfo.Add(firstShip);
        }
        else
        {
            GetShipsData();

            ShipInfo lastInfo = savedShipsInfo.Last();
            bool isLastShipQuantityEnough = lastInfo.quantity >= lastInfo.shipData.qtForNextShip;
            if (isLastShipQuantityEnough)
            {
                bool isVeryLastShip = lastInfo.index + 1 >= totalShips.Count;
                if (!isVeryLastShip)
                {
                    int index = lastInfo.index + 1;
                    ShipInfo newShip = new ShipInfo(index, totalShips[index], 0, new List<UpgradeInfo>());

                    savedShipsInfo.Add(newShip);
                }
            }
        }
        EventSendData?.Invoke(savedShipsInfo, this);

    }

    public void UnlockNewShip(ShipData shipData)
    {
        int index = totalShips.IndexOf(shipData) + 1;

        if (index < totalShips.Count)
        {
            ShipInfo newShipInfo = new ShipInfo(totalShips[index].index, totalShips[index], 0, new List<UpgradeInfo>());
            EventUnlockShip?.Invoke(newShipInfo, this);
        }
    }

    public void SpawnShipModel(ShipData shipData)
    {
        if (shipsModel.Count == 1)
        {
            canvasBottom.ShowCycleButtons(); // TODO: Remove and use the Event above
        }

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
        GameObject shipModel = shipsModel[currentModelIndex];

        shipModel.transform.SetParent(null);
        shipModel.transform.position = shipsStartingPosition;
    }

    public void ViewShip()
    {
        GameObject shipModel = shipsModel[currentModelIndex];

        shipModel.transform.SetParent(shipsParent);
        shipModel.transform.localPosition = Vector3.zero;
        shipModel.transform.rotation = shipsParent.rotation;

        EventShipViewChanged?.Invoke(currentModelIndex);
    }

    public void SaveData()
    {
        List<Ship> ships = canvasBottom.Ships;
        List<ShipInfo> shipsInfo = new List<ShipInfo>();
        ShipInfo shipInfo;

        for (int i = 0; i < ships.Count; ++i)
        {
            shipInfo = new ShipInfo(ships[i].shipData.index, ships[i].shipData, ships[i].Quantity, ships[i].UpgradesInfo);
            shipsInfo.Add(shipInfo);
        }

        SaveManager.PlayerData.ships = shipsInfo;
    }

    private void GetShipsData()
    {
        totalShips = totalShips.OrderBy(ship => ship.index).ToList();
        totalUpgrades = totalUpgrades.OrderBy(upgrade => upgrade.index).ToList();

        ShipInfo shipInfo;
        ShipData shipData;
        List<UpgradeInfo> upgrades;
        UpgradeInfo upgradeInfo;
        UpgradeData upgradeData;

        for (int i = 0; i < savedShipsInfo.Count; ++i)
        {
            shipInfo = savedShipsInfo[i];
            shipData = null;
            upgrades = new List<UpgradeInfo>();
            upgradeInfo = new UpgradeInfo();
            upgradeData = null;

            shipData = totalShips[shipInfo.index];

            for (int j = 0; j < shipInfo.upgradesInfo.Count; ++j)
            {
                upgradeInfo = shipInfo.upgradesInfo[j];

                upgradeData = totalUpgrades[upgradeInfo.index];

                upgradeInfo = new UpgradeInfo(upgradeInfo.index, upgradeData, upgradeInfo.isOwned);

                upgrades.Add(upgradeInfo);
            }

            shipInfo = new ShipInfo(shipInfo.index, shipData, shipInfo.quantity, upgrades);

            savedShipsInfo[i] = shipInfo;
        }
    }
}
