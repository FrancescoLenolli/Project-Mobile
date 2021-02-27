using System;
using System.Collections.Generic;

public class PlayerData
{
    // GAME DATA
    // ********** GENERAL **********
    //public bool isFirstSession = true;
    public double currency = 0;
    public int premiumCurrency = 0;
    public List<ShipInfo> ships = new List<ShipInfo>();
    //public double currencyIdleGain = 0;
    //public float modifierIdleGain = 0;
    public string lastLogOutTime = "";
    public double secondsDoubleGain = 0;

    ////********** SETTINGS **********
    public bool isVolumeSFXOn = true;
    public bool isVolumeMusicOn = true;
    public bool isVibrationOn = true;

    ////********** SHIPS & UPGRADES **********
    //public List<ShipInfo> listShipInfos = null;
    //public List<UpgradeInfo> listUpgradesUnlocked = null;
    //public List<UpgradeInfo> listUpgradesBought = null;

    ////********** SHIPS 3D MODEL **********
    //public  int unlockedShipsCount = 0;

    ////********** DAILY REWARDS **********
    //public List<int> listRewardsIndexes = null;
    //public int currentRewardIndex = 0;
    //public long collectionCooldownTime = 0;

}
