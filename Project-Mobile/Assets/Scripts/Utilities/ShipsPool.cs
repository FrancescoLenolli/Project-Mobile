using System.Collections.Generic;
using UnityEngine;

public class ShipsPool : MonoBehaviour
{
    [SerializeField] private Ship shipPrefab = null;
    [SerializeField] private Upgrade upgradePrefab = null;

    private Stack<Ship> ships = new Stack<Ship>();
    private Stack<Upgrade> upgrades = new Stack<Upgrade>();
    private Vector3 startingPosition = new Vector3(0f, 0f, -500f);
    private Quaternion startingRotation = Quaternion.identity;

    public void InitPool(List<ShipData> shipsData)
    {
        foreach (ShipData shipData in shipsData)
        {
            Ship ship = Instantiate(shipPrefab, startingPosition, startingRotation, transform);
            ships.Push(ship);

            foreach (UpgradeData upgradeData in shipData.upgrades)
            {
                Upgrade upgrade = Instantiate(upgradePrefab, startingPosition, startingRotation, transform);
                upgrades.Push(upgrade);
            }
        }
    }

    public Ship GetShip()
    {
        return ships.Pop();
    }

    public Upgrade GetUpgrade()
    {
        return upgrades.Pop();
    }

    public void CollectUpgrade(Upgrade upgrade)
    {
        upgrade.transform.SetParent(transform);
        upgrade.transform.position = startingPosition;

        upgrades.Push(upgrade);
    }
}
