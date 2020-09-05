using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the informations needed for a Ship, like his Data and his Quantity.
/// </summary>
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
public class ShipsManager : Singleton<ShipsManager>
{
    private List<ShipData> listShipDatas = new List<ShipData>();
    private List<Ship> listShips = new List<Ship>();

    /// List that stores the information to be saved.
    public List<ShipInfo> listShipInfos = new List<ShipInfo>();
    public Ship prefabShip = null;
    public Transform containerShips = null; // [!!!] I don't like this, better to have this in a UIManager and pass it that way?

    private new void Awake()
    {
        base.Awake();
        listShipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));
    }

    private void Start()
    {
        SaveManager.Instance.Load();
        InitShips();
    }

    // When the game start, Instantiate the ships owned by the player.
    // If this is the first time the game is launched, Instantiate only the first ship with quantity 0.
    private void InitShips()
    {
        // Get Saved ShipsInfo.
        listShipInfos = SaveManager.Instance.playerData.playerShips;

        // Handles first time the game is played by adding the first type of ship.
        if (listShipInfos.Count == 0)
        {
            ShipInfo firstShipInfo = new ShipInfo(listShipDatas[0], 0);
            listShipInfos.Add(firstShipInfo);
        }

        // Spawn all ships currently unlocked.
        for (int i = 0; i < listShipInfos.Count; ++i)
        {
            ShipData newData = listShipInfos[i].shipData;
            int newQuantity = listShipInfos[i].shipQuantity;

            Ship newShip = Instantiate(prefabShip, containerShips, false);
            newShip.SetValues(newData, newQuantity);

            listShips.Add(newShip);
        }
    }

    // Spawn new type of Ship.
    // This happens when the previous type of Ship is owned in sufficient quantity.
    public void AddNewShip(int currentShipIndex)
    {
        // If the ship that called this function is the last on the list, unlock the next one.
        if (currentShipIndex + 1 == listShipInfos.Count)
        {
            // If the two lists have equal count, all ships have been unlocked...
            if (listShipInfos.Count == listShipDatas.Count)
            {
                Debug.Log("You unlocked every ship!");
            }

            // ...If not, get the data for the next ship, and spawn it.
            else
            {
                ShipData newData = listShipDatas[listShipInfos.Count];
                int newQuantity = 0;

                InstantiateShip(newData, newQuantity);
            }
        }
    }

    /// Instantiate new Ship and update Ship and ShipInfo lists.
    private void InstantiateShip(ShipData newShipData, int newQuantity = 0)
    {
        Ship newShip = Instantiate(prefabShip, containerShips, false);
        ShipInfo newShipInfo = new ShipInfo(newShipData, newQuantity);

        newShip.SetValues(newShipData, newQuantity);

        listShips.Add(newShip);
        listShipInfos.Add(newShipInfo);
    }

    // [!!!] I don't see another way to update the quantity for now.
    // Set Quantities for every ShipInfo to be saved.
    public void SetQuantities()
    {
        for (int i = 0; i < listShipInfos.Count; ++i)
        {
            listShipInfos[i] = new ShipInfo(listShipInfos[i].shipData, listShips[i].quantity);
        }
    }
}
