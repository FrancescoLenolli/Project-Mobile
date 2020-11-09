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

    // Modifier expressed in percentage, modify the Ship's Currency Gain.
    public int shipIdleGainModifier;

    public ShipInfo(ShipData data, int quantity, int multiplier)
    {
        shipData = data;
        shipQuantity = quantity;
        shipIdleGainModifier = multiplier;

    }
}

public delegate void ShipsInitialised();

public class PanelShips : MonoBehaviour
{
    public event ShipsInitialised eventShipsInitialised;

    private GameManager gameManager = null;
    private UIManager uiManager = null;
    private CanvasBottom canvasBottom = null;
    private List<ShipData> listShipDatas = new List<ShipData>();
    private List<Ship> listShips = new List<Ship>();
    private RectTransform containerShipsRect = null;

    [SerializeField] private Ship prefabShip = null;
    [SerializeField] private Transform containerShips = null;

    [HideInInspector] public List<ShipInfo> listShipInfos = new List<ShipInfo>();

    // Initialise data and Instantiate all ships owned by the player at the START OF THE GAME.
    public void InitShips()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        canvasBottom = FindObjectOfType<CanvasBottom>();
        containerShipsRect = containerShips.GetComponent<RectTransform>();
        listShipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));

        eventShipsInitialised += canvasBottom.panelShipsUpgrades.InitUpgrades;

        // Update the list of ships to spawn with Saved Data.
        listShipInfos = gameManager.playerData.playerShips;

        // First time the player play the game.
        // If the player has no ships, add the first type to list.
        if (listShipInfos.Count == 0)
        {
            ShipInfo firstShipInfo = new ShipInfo(listShipDatas[0], 0, 0);
            listShipInfos.Add(firstShipInfo);
        }

        // Spawn all ships owned by the player.
        for(int i = 0; i < listShipInfos.Count; ++i)
        {
            ShipData newData = listShipInfos[i].shipData;
            int newQuantity = listShipInfos[i].shipQuantity;
            int newMultiplier = listShipInfos[i].shipIdleGainModifier;

            Ship newShip = Instantiate(prefabShip, containerShips, false);
            newShip.SetValues(newData, newQuantity, newMultiplier);

            listShips.Add(newShip);

            // Resize ships container adding the ship's height.
            containerShipsRect.sizeDelta = uiManager.ResizeContainer(containerShips, newShip.transform, 10);
            // Put the new Ship at the top of the list inside the UI.
            newShip.transform.SetSiblingIndex(0);
        }

        // Send a message when every Ship owned by the Player has been spawned.
        eventShipsInitialised?.Invoke();
    }

    // Instantiate new ship, add it to listShips and listShipInfos, and update shipsContainer.
    private void InitAndAddShip(ShipData newShipData)
    {
        // Starting quantity and multiplier will always be 0.
        int newQuantity = 0;
        int newMultiplier = 0;

        Ship newShip = Instantiate(prefabShip, containerShips, false);
        newShip.SetValues(newShipData, newQuantity, newMultiplier);
        ShipInfo newShipInfo = new ShipInfo(newShipData, newQuantity, newMultiplier);

        listShips.Add(newShip);
        listShipInfos.Add(newShipInfo);

        containerShipsRect.sizeDelta = uiManager.ResizeContainer(containerShips, newShip.transform, 10);
        newShip.transform.SetSiblingIndex(0);
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

    // Update quantity of specific ship.
    public void UpdateQuantityAt(int shipIndex, int newQuantity)
    {
        listShipInfos[shipIndex] = new ShipInfo(listShipInfos[shipIndex].shipData, newQuantity, listShipInfos[shipIndex].shipIdleGainModifier);
    }

    public void UpdateModifierAt(int shipIndex, int newModifier)
    {
        listShipInfos[shipIndex] = new ShipInfo(listShipInfos[shipIndex].shipData, listShipInfos[shipIndex].shipQuantity, newModifier);
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
