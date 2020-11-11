﻿using System;
using System.Collections.Generic;

public class PlayerData
{
    // DEBUG DATA
    public bool canSaveData = true;
    public bool canDebug = false;

    // GAME DATA
    public string playerName = "PlayerName";
    public long playerCurrency = 0;
    public bool SFXVolume = true;
    public bool MusicVolume = true;
    public bool VibrationOn = true;
    public List<ShipInfo> playerShips = null;
    public List<UpgradeInfo> playerUpgradesUnlocked = null;
    public List<UpgradeInfo> playerUpgradesBought = null;
    public int lastCurrencyIdleGain = 0;
    public int lastModifierIdleGain = 0;
    public string lastPlayedTime = "";
}
