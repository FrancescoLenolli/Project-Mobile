using System;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/ShipData", fileName = "New Ship")]
public class ShipData : ScriptableObject
{
    public enum ShipType { Patrol, Light, Heavy, Corvette, ManOfWar, Galleon, Battleship, Titan, DeathStar, BorgCube }

    public ShipType shipType = ShipType.Patrol;
    public int index = 0;
    public string shipName = "New Ship";
    public string shipDescription = "Ship Description";
    public Sprite shipIcon = null;
    public GameObject shipPrefab = null;
    public double cost = 0;
    public double currencyGain = 0;
    // Defined by upgrades, how much is increased currencyGain.
    // Expressed in percentages, a value of 50 increase currencyGain by 50%;
    public int productionMultiplier = 0;

    [Tooltip("How many units of this ship do I need to unlock the next Type?")]
    public int qtToUnlockNextShip = 0;
}
