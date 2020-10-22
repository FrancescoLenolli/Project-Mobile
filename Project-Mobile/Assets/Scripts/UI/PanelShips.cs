using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShipInfo
{
    /// Generic Data for the Ship.
    /// Things like Name, Sprite, Currency Gain ecc... are all contained here.
    public ShipData shipData;

    /// How many ships of the same type the player has.
    public int shipQuantity;

    public ShipInfo(ShipData data, int quantity)
    {
        shipData = data;
        shipQuantity = quantity;
    }
}

public class PanelShips : MonoBehaviour
{
    private GameManager gameManager = null;
    private UIManager uiManager = null;
    private List<ShipData> listShipDatas = new List<ShipData>();
    private List<Ship> listShips = new List<Ship>();

    [HideInInspector] public List<ShipInfo> listShipInfos = new List<ShipInfo>();

    [SerializeField] private Ship prefabShip = null;
    [SerializeField] private Transform containerShips = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;

        listShipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));
        InitShips();
    }

    // ONLY AT LAUNCH.
    // Instantiate all ships owned by the player.
    private void InitShips()
    {
        // Update the list of ships to spawn with Saved Data.
        listShipInfos = gameManager.playerData.playerShips;

        // First time the player play the game.
        // If the player has no ships, add the first type to list.
        if (listShipInfos.Count == 0)
        {
            ShipInfo firstShipInfo = new ShipInfo(listShipDatas[0], 0);
            listShipInfos.Add(firstShipInfo);
        }

        // Spawn all ships owned by the player.
        for(int i = 0; i < listShipInfos.Count; ++i)
        {
            ShipData newData = listShipInfos[i].shipData;
            int newQuantity = listShipInfos[i].shipQuantity;

            Ship newShip = Instantiate(prefabShip, containerShips, false);
            newShip.SetValues(newData, newQuantity); // [!!!] ADD "this" to SetValues parameters to pass reference to PanelShips.

            listShips.Add(newShip);

            // Resize ships container adding the ship's height.
            // Better than having a fixed size container.
            RectTransform containerShipsRect = containerShips.GetComponent<RectTransform>();
            containerShipsRect.sizeDelta = uiManager.ResizeContainer(containerShips, newShip.transform, 10);
            newShip.transform.SetSiblingIndex(0);
        }
    }

    // If conditions are met, unlock new type of ship.
    public void AddNewShip(int currentShipIndex)
    {
        bool canAddShip = (currentShipIndex + 1 == listShipInfos.Count && listShipInfos.Count != listShipDatas.Count);

        // Spawn next type of ship.
        if(canAddShip)
        {
            ShipData newData = listShipDatas[listShipInfos.Count];

            InitAndAddShip(newData);
        }

        gameManager.SaveShipInfos(listShipInfos);
    }
    
    // Instantiate new ship, add it to listShips and listShipInfos, and update shipsContainer.
    private void InitAndAddShip(ShipData newShipData)
    {
        // Starting quantity will always be 0.
        int newQuantity = 0;

        Ship newShip = Instantiate(prefabShip, containerShips, false);
        newShip.SetValues(newShipData, newQuantity);
        ShipInfo newShipInfo = new ShipInfo(newShipData, newQuantity);

        listShips.Add(newShip);
        listShipInfos.Add(newShipInfo);

        // [!!!] This can also be useful in Upgrades Panel, consider making it a function in UIManager.
        RectTransform containerShipsRect = containerShips.GetComponent<RectTransform>();
        containerShipsRect.sizeDelta = uiManager.ResizeContainer(containerShips, newShip.transform, 10);
        newShip.transform.SetSiblingIndex(0);
    }

    // Update quantity of specific ship.
    public void UpdateQuantityAt(int shipIndex, int newQuantity)
    {
        listShipInfos[shipIndex] = new ShipInfo(listShipInfos[shipIndex].shipData, newQuantity);
    }

    public Ship ReturnShipOfType(ShipData.ShipType type)
    {
        Ship ship = null;

        for (int i = 0; i < listShips.Count; ++i)
        {
            if(listShips[i].shipData.shipType == type)
            {
                ship = listShips[i];
                break;
            }
        }

        return ship;
    }
}
