using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the informations needed for a Ship, like his Data and his Quantity.
/// </summary>
[System.Serializable]
public struct ShipInfo
{
    /// <summary>
    /// Generic Data for the Ship.
    /// Things like Name, Sprite, Currency Gain ecc... are all contained here.
    /// </summary>
    public ShipData shipData;

    /// <summary>
    /// How many ships of the same type the player has.
    /// </summary>
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
    /// <summary>
    /// List of Ship Infos to Save.
    /// </summary>
    public List<ShipInfo> listShipInfos = new List<ShipInfo>();
    List<Ship> listShips = new List<Ship>();
    Dictionary<int, ShipInfo> dctnShipsInfo = new Dictionary<int, ShipInfo>();

    public Ship prefabShip = null;
    public Transform containerShips = null;

    private new void Awake()
    {
        base.Awake();
        listShipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));
    }

    private void Start()
    {
        InstantiateShips();
    }

    private void InstantiateShips()
    {
        // Handles first time the game is played by adding the first type of Ship.
        if (listShipInfos.Count == 0)
        {
            ShipInfo firstShipInfo = new ShipInfo(listShipDatas[0], 0);
            listShipInfos.Add(firstShipInfo);
        }
        // Spawn all ships currently unlocked.
        for (int i = 0; i < dctnShipsInfo.Count; ++i)
        {
            ShipData newData = dctnShipsInfo[i].shipData;
            int newQuantity = dctnShipsInfo[i].shipQuantity;

            Ship newShip = Instantiate(prefabShip, containerShips, false);
            newShip.SetValues(newData, newQuantity);
        }
    }

    /// <summary>
    /// Add New Ship with ShipData taken from listShipDatas[newShipIndex].
    /// </summary>
    /// <param name="newShipIndex"></param>
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

    /// <summary>
    /// Instantiate new Ship and store a new ShipInfo to Save.
    /// </summary>
    /// <param name="newShipData"></param>
    /// <param name="newQuantity"></param>
    private void InstantiateShip(ShipData newShipData, int newQuantity = 0)
    {
        Ship newShip = Instantiate(prefabShip, containerShips, false);
        ShipInfo newShipInfo = new ShipInfo(newShipData, newQuantity);

        newShip.SetValues(newShipData, newQuantity);

        listShips.Add(newShip);
        listShipInfos.Add(newShipInfo);
    }
}
