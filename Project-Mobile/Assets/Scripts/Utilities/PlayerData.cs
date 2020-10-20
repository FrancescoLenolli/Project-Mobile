using System;
using System.Collections.Generic;

public class PlayerData
{
    public string playerName = "PlayerName";
    public long playerCurrency = 0;
    public bool SFXVolume = true;
    public bool MusicVolume = true;
    public bool VibrationOn = true;
    public List<ShipInfo> playerShips = new List<ShipInfo>();
    public int lastCurrencyIdleGain = 0;
    public int lastModifierIdleGain = 0;
    public string lastPlayedTime;
}
