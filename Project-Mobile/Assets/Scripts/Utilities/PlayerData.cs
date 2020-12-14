using System;
using System.Collections.Generic;

public class PlayerData
{
    // GAME DATA
    // ********** GENERAL **********
    public bool isFirstSession = true;
    public string playerName = "PlayerName";
    public long playerCurrency = 0;
    public long lastCurrencyIdleGain = 0;
    public int lastModifierIdleGain = 0;
    public string lastPlayedTime = "";

    //********** SETTINGS **********
    public bool SFXVolumeOn = true;
    public bool MusicVolumeOn = true;
    public bool VibrationOn = true;

    //********** SHIPS & UPGRADES **********
    public List<ShipInfo> playerShips = null;
    public List<UpgradeInfo> playerUpgradesUnlocked = null;
    public List<UpgradeInfo> playerUpgradesBought = null;

    //********** DAILY REWARDS **********
    public List<int> listRewardsIndexes = null;
    public int currentRewardIndex = 0;
    public long rewardCooldownTime = 0;

}
