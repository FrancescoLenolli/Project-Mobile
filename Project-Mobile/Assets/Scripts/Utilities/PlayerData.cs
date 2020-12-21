using System;
using System.Collections.Generic;

public class PlayerData
{
    // GAME DATA
    // ********** GENERAL **********
    public bool isFirstSession = true;
    public string playerName = "PlayerName";
    public double playerCurrency = 0;
    public double lastCurrencyIdleGain = 0;
    public float lastModifierIdleGain = 0;
    public string lastPlayedTime = "";
    public int timeDoubledIdleGain = 0;

    //********** SETTINGS **********
    public bool SFXVolumeOn = true;
    public bool MusicVolumeOn = true;
    public bool VibrationOn = true;

    //********** SHIPS & UPGRADES **********
    public List<ShipInfo> playerShips = null;
    public List<UpgradeInfo> playerUpgradesUnlocked = null;
    public List<UpgradeInfo> playerUpgradesBought = null;

    //********** SHIPS 3D MODEL **********
    public  int unlockedShipsCount = 0;

    //********** DAILY REWARDS **********
    public List<int> listRewardsIndexes = null;
    public int currentRewardIndex = 0;
    public long rewardCooldownTime = 0;

}
