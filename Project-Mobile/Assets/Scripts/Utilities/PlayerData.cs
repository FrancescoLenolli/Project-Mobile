using System;
using System.Collections.Generic;

public class PlayerData
{
    // DEBUG
    public bool canSaveData = true;
    public bool canDebug = false;
    //**********

    // GAME DATA
    public string playerName = "PlayerName";
    public long playerCurrency = 0;
    public bool SFXVolume = true;
    public bool MusicVolume = true;
    public bool VibrationOn = true;
    public List<ShipInfo> playerShips = new List<ShipInfo>();
    public List<UpgradeInfo> playerUpgradesUnlocked = new List<UpgradeInfo>();
    public List<UpgradeInfo> playerUpgradesBought = new List<UpgradeInfo>();
    public int lastCurrencyIdleGain = 0;
    public int lastModifierIdleGain = 0;
    public string lastPlayedTime = "";
    //**********

    public PlayerData(int currency, int lastIdleGain, int lastModifierIdle, string lastTime)
    {
        playerCurrency = currency;
        lastCurrencyIdleGain = lastIdleGain;
        lastModifierIdleGain = lastModifierIdle;
        lastPlayedTime = lastTime;
    }

    public PlayerData()
    {
    }
}
