using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShipInfo
{
    /// Generic Data for the Ship.
    /// Things like Name, Sprite, Currency Gain ecc... are all contained here.
    public string shipName;

    /// How many ships of the same type the player has.
    public int shipQuantity;

    // Modifier expressed in percentage, modify the Ship's Currency Gain.
    public int shipIdleGainModifier;

    public ShipInfo(string name, int quantity, int multiplier)
    {
        shipName = name;
        shipQuantity = quantity;
        shipIdleGainModifier = multiplier;

    }
}

public delegate void ShipsInitialised();
public class PanelShips : MonoBehaviour
{
    public event ShipsInitialised EventShipsInitialised;

    private GameManager gameManager = null;
    private UIManager uiManager = null;
    private CanvasBottom canvasBottom = null;
    private List<ShipData> listShipDatas = new List<ShipData>();
    private List<Ship> listShips = new List<Ship>();
    private RectTransform containerShipsRect = null;

    [SerializeField] private Ship prefabShip = null;
    [SerializeField] private Transform containerShips = null;

    [HideInInspector] public List<ShipInfo> listShipInfos = new List<ShipInfo>();

    // Instantiate new ship, add it to listShips and listShipInfos, and update shipsContainer's size.
    private void InitAndAddShip(string shipName)
    {
        // Starting quantity and multiplier will always be 0.
        int newQuantity = 0;
        int newMultiplier = 0;
        ShipData newShipData = ReturnShipData(shipName);

        Ship newShip = Instantiate(prefabShip, containerShips, false);
        newShip.SetValues(newShipData, newQuantity, newMultiplier);
        ShipInfo newShipInfo = new ShipInfo(shipName, newQuantity, newMultiplier);

        listShips.Add(newShip);
        listShipInfos.Add(newShipInfo);

        containerShipsRect.sizeDelta = uiManager.ResizeContainer(containerShips, newShip.transform);
        newShip.transform.SetSiblingIndex(0);
    }

    // Return a ShipData from the list of ShipDatas.
    private ShipData ReturnShipData(string shipDataName)
    {
        ShipData result = null;

        foreach(ShipData data in listShipDatas)
        {
            if (data.shipName == shipDataName)
            {
                result = data;
                break;
            }
        }

        return result;
    }

    // Initialise data and Instantiate all ships owned by the player at the START OF THE GAME.
    public void InitShips()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        canvasBottom = GetComponentInParent<CanvasBottom>();
        containerShipsRect = containerShips.GetComponent<RectTransform>();
        listShipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));

        EventShipsInitialised += canvasBottom.panelShipsUpgrades.InitUpgrades;

        // Update the list of ships to spawn with Saved Data.
        listShipInfos = gameManager.playerData.playerShips;

        // Handles first time the game is played.
        // If the player has no ships, add the first type to list.
        if (listShipInfos == null)
        {
            listShipInfos = new List<ShipInfo>();
            ShipInfo firstShipInfo = new ShipInfo(listShipDatas[0].shipName, 0, 0);
            listShipInfos.Add(firstShipInfo);
        }

        // Spawn all ships owned by the player.
        for (int i = 0; i < listShipInfos.Count; ++i)
        {
            ShipData newData = ReturnShipData(listShipInfos[i].shipName);
            int newQuantity = listShipInfos[i].shipQuantity;
            int newMultiplier = listShipInfos[i].shipIdleGainModifier;

            Ship newShip = Instantiate(prefabShip, containerShips, false);
            newShip.SetValues(newData, newQuantity, newMultiplier);

            listShips.Add(newShip);

            // Resize ships container adding the ship's height.
            containerShipsRect.sizeDelta = uiManager.ResizeContainer(containerShips, newShip.transform);
            // Put the new Ship at the top of the list inside the UI.
            newShip.transform.SetSiblingIndex(0);
        }

        // When every ship is spawned, start instantiating the upgrades
        EventShipsInitialised?.Invoke();
    }

    /// <summary>
    /// Unlock new Ship.
    /// </summary>
    /// <param name="currentShipIndex"></param>
    public void UnlockNewShip(int currentShipIndex)
    {
        // currentShipIndex is the index of the last ship bought.
        // index + 1 is the index of the next ship.
        // If this new Index is equal to listShipInfo count, the ship with that index is not yet instantiated.
        // it this listShipInfo count is equal to listShipData count, every ship has already been instantiated.
        bool canAddShip = (currentShipIndex + 1 == listShipInfos.Count && listShipInfos.Count != listShipDatas.Count);

        // Spawn next type of ship.
        if(canAddShip)
        {
            string newDataName = listShipDatas[listShipInfos.Count].shipName;

            InitAndAddShip(newDataName);
        }
    }

    /// <summary>
    /// Update quantity value of specific ship.
    /// </summary>
    /// <param name="shipIndex"></param>
    /// <param name="newQuantity"></param>
    public void UpdateQuantityAt(int shipIndex, int newQuantity)
    {
        listShipInfos[shipIndex] = new ShipInfo(listShipInfos[shipIndex].shipName, newQuantity, listShipInfos[shipIndex].shipIdleGainModifier);
    }

    /// <summary>
    /// Update currencyGain modifier value of specific ship.
    /// </summary>
    /// <param name="shipIndex"></param>
    /// <param name="newModifier"></param>
    public void UpdateModifierAt(int shipIndex, int newModifier)
    {
        listShipInfos[shipIndex] = new ShipInfo(listShipInfos[shipIndex].shipName, listShipInfos[shipIndex].shipQuantity, newModifier);
    }

    /// <summary>
    /// Return an object of type Ship from the list of Ships owned.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
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

    public void SaveShipsInfo()
    {
        gameManager.playerData.playerShips = listShipInfos;
    }
}
