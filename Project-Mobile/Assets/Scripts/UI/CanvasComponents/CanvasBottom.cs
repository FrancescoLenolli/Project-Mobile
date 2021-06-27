using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBottom : MonoBehaviour
{
    public Action<UtilsUI.Cycle> EventCycleShipsModel;
    public Action EventShowDailyRewards;

    [SerializeField] private Transform containersParent = null;
    [SerializeField] private Transform containerShips = null;
    [SerializeField] private Transform containerUpgrades = null;
    [SerializeField] private List<Transform> cycleButtons = null;
    [SerializeField] private PanelExtra panelExtra = null;
    [SerializeField] private PanelAnimator panelExtraAnimator = null;
    [SerializeField] private PanelAnimator panelBottomAnimator = null;
    [SerializeField] private PanelShipInfo panelShipInfo = null;

    private CurrencyManager currencyManager;
    private PanelPrestige panelPrestige;
    private List<Transform> containers = new List<Transform>();
    private List<Ship> ships = new List<Ship>();

    public List<Ship> Ships { get => ships; }
    public PanelShipInfo PanelInfo { get => panelShipInfo; }

    public void InitData(List<ShipInfo> shipsInfo, ShipsManager shipsManager)
    {
        currencyManager = CurrencyManager.Instance;
        panelPrestige = FindObjectOfType<PanelPrestige>();
        CanvasDailyRewards canvasDailyRewards = FindObjectOfType<CanvasDailyRewards>();
        SwipeDetector swipeDetector = FindObjectOfType<SwipeDetector>();

        panelExtra.InitData(this);

        containersParent.GetComponentsInChildren<ScrollRect>().ToList().ForEach(container => containers.Add(container.transform));

        OpenPanel(0);

        for (int i = 0; i < shipsInfo.Count; ++i)
        {
            SpawnShip(shipsInfo[i], shipsManager);

            if (i == shipsInfo.Count - 1 && shipsInfo[i].quantity > 0)
            {
                shipsManager.ViewShip();
            }
        }

        Observer.AddObserver(ref EventCycleShipsModel, shipsManager.CycleModels);
        Observer.AddObserver(ref EventShowDailyRewards, canvasDailyRewards.MoveToPosition);
        Observer.AddObserver(ref swipeDetector.EventSwipe, ChangeBottomPanelVisibility);
    }

    public void CycleModelsLeft()
    {
        EventCycleShipsModel?.Invoke(UtilsUI.Cycle.Left);
    }

    public void CycleModelsRight()
    {
        EventCycleShipsModel?.Invoke(UtilsUI.Cycle.Right);
    }

    public void ShowCycleButtons()
    {
        UtilsUI.ChangeVisibility(cycleButtons, true);
    }

    public void SpawnShip(ShipInfo shipInfo, ShipsManager shipsManager)
    {
        Ship ship = shipsManager.ShipsPool.GetShip();
        ship.InitData(shipInfo, shipsManager, this, panelPrestige, containerShips);
        if (ship.Quantity > 0)
            SpawnUpgrades(ship, shipsManager.ShipsPool);

        currencyManager.AddCollectible(ship);
        ships.Add(ship);

        ship.transform.SetAsFirstSibling();
        UtilsUI.ResizeContainer(ship.transform, containerShips, UtilsUI.Resize.Add);
    }

    public void SpawnUpgrades(Ship ship, ShipsPool shipsPool)
    {
        foreach (UpgradeInfo info in ship.UpgradesInfo.Where(x => !x.isOwned))
        {
            Upgrade upgrade = shipsPool.GetUpgrade();
            upgrade.InitData(info.upgradeData, ship, containerUpgrades, panelShipInfo);

            upgrade.transform.SetAsFirstSibling();
            UtilsUI.ResizeContainer(upgrade.transform, containerUpgrades, UtilsUI.Resize.Add);
        }
    }

    public void OpenPanel(int index)
    {
        UtilsUI.ChangeVisibility(containers, index);
        panelBottomAnimator.MoveToView();
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
            panelExtraAnimator.HidePanel();
        else
            panelExtraAnimator.ShowPanel();
    }

    private void ChangeBottomPanelVisibility(SwipeDetector.Swipe swipe, Vector2 startPosition)
    {
        switch (swipe)
        {
            case SwipeDetector.Swipe.Down:
                panelBottomAnimator.HideFromView();
                break;
            case SwipeDetector.Swipe.Up:
                panelBottomAnimator.MoveToView();
                break;
            default:
                break;
        }
    }
}
