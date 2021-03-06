﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBottom : MonoBehaviour
{
    private Action<UIManager.Cycle> EventCycleShipsModel;
    private Action EventShowDailyRewards;

    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private List<Transform> containers = new List<Transform>();
    private List<Ship> ships = new List<Ship>();

    [SerializeField] private Ship prefabShip = null;
    [SerializeField] private Upgrade prefabUpgrade = null;
    [SerializeField] private Transform containersParent = null;
    [SerializeField] private Transform containerShips = null;
    [SerializeField] private Transform containerUpgrades = null;
    [SerializeField] private List<Transform> cycleButtons = null;
    [SerializeField] private PanelExtra panelExtra = null;
    [SerializeField] private PanelAnimator panelAnimator = null;

    public List<Ship> Ships { get => ships; }

    public void InitData(List<ShipInfo> shipsInfo, ShipsManager shipsManager)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;
        panelExtra.InitData(this);

        List<ScrollRect> list = containersParent.GetComponentsInChildren<ScrollRect>().ToList();
        list.ForEach(x => containers.Add(x.transform));

        OpenPanel(0);

        CanvasDailyRewards canvasDailyRewards = FindObjectOfType<CanvasDailyRewards>();

        SubscribeToEventCycleShipsModel(shipsManager.CycleModels);
        SubscribeToEventShowDailyRewards(canvasDailyRewards.MoveToPosition);

        for (int i = 0; i < shipsInfo.Count; ++i)
        {
            SpawnShip(shipsInfo[i], shipsManager);

            if (i == shipsInfo.Count - 1 && shipsInfo[i].quantity > 0)
            {
                shipsManager.ViewShip();
            }
        }
    }

    public void CycleModelsLeft()
    {
        EventCycleShipsModel?.Invoke(UIManager.Cycle.Left);
    }

    public void CycleModelsRight()
    {
        EventCycleShipsModel?.Invoke(UIManager.Cycle.Right);
    }

    public void ShowCycleButtons()
    {
        UIManager.Instance.ChangeVisibility(cycleButtons, true);
    }

    public void SpawnShip(ShipInfo shipInfo, ShipsManager shipsManager)
    {
        Ship ship = Instantiate(prefabShip, containerShips, false);
        ship.InitData(shipInfo, shipsManager, this);
        if (ship.Quantity > 0)
            SpawnUpgrades(ship);

        currencyManager.AddCollectible(ship);
        ships.Add(ship);

        ship.transform.SetAsFirstSibling();
        uiManager.ResizeContainer(ship.transform, containerShips, UIManager.Resize.Add);
    }

    public void OpenPanel(int index)
    {
        uiManager.ChangeVisibility(containers, index);
    }

    public void AddExtraCurrency()
    {
        MovePanelToPosition(false);
        panelExtra.SetUpPanel(AdsManager.AdType.BaseCurrency);
    }

    public void AddExtraDoubleGainTime()
    {
        MovePanelToPosition(false);
        panelExtra.SetUpPanel(AdsManager.AdType.DoubleIdleEarnings);
    }

    public void AddExtraPremiumCurrency()
    {
        MovePanelToPosition(false);
        panelExtra.SetUpPanel(AdsManager.AdType.PremiumCurrency);
    }

    public void ShowDailyRewards()
    {
        EventShowDailyRewards?.Invoke();
    }

    public void MovePanelToPosition(bool isPanelVisible)
    {
        if (isPanelVisible)
            panelAnimator.HidePanel();
        else
            panelAnimator.ShowPanel();
    }

    public void SpawnUpgrades(Ship ship)
    {
        foreach (UpgradeInfo info in ship.UpgradesInfo.Where(x => !x.isOwned))
        {
            Upgrade upgrade = Instantiate(prefabUpgrade, containerUpgrades, false);
            upgrade.InitData(info.upgradeData, ship, containerUpgrades);

            upgrade.transform.SetAsFirstSibling();
            uiManager.ResizeContainer(upgrade.transform, containerUpgrades, UIManager.Resize.Add);
        }
    }

    public void SubscribeToEventCycleShipsModel(Action<UIManager.Cycle> method)
    {
        EventCycleShipsModel += method;
    }

    public void SubscribeToEventShowDailyRewards(Action method)
    {
        EventShowDailyRewards += method;
    }
}
