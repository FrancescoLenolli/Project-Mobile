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
        this.shipData = data;
        this.shipQuantity = quantity;
    }
}
public class ShipsManager : Singleton<ShipsManager>
{
    List<ShipData> listShipDatas = new List<ShipData>();
    /// List of Ship Infos to Save.
    public List<ShipInfo> listShipInfos = new List<ShipInfo>();
    List<Ship> listShips = new List<Ship>();

    public Ship prefabShip = null;
    public Transform containerShips = null;

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

    private void InitShips()
    {
        // Get Saved ShipsInfo.
        listShipInfos = SaveManager.Instance.playerData.playerShips;

        // Handles first time the game is played by adding the first type of Ship.
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

    /// Add New Ship with ShipData taken from listShipDatas[newShipIndex].
    public void AddNewShip(int currentShipIndex)
    {
        // If the ship that called this function is the last on the list, unlock the next one.
        if (currentShipIndex + 1 == listShipInfos.Count)
        {
            // If the two lists have equal count, all ships have been unlocked.
            if (listShipDatas.Count != listShipInfos.Count)
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

    // Set Quantities for every ShipInfo to be saved.
    public void SetQuantities()
    {
        for(int i = 0; i < listShipInfos.Count; ++i)
        {
            listShipInfos[i] = new ShipInfo(listShipInfos[i].shipData, listShips[i].quantity);
        }
    }
}
