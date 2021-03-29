using System.Collections.Generic;

public class PlayerData
{
    // ********** GENERAL **********
    //public bool isFirstSession = true;
    public double currency = 0;
    public int premiumCurrency = 0;
    public int prestigeLevel = 1;
    public List<ShipInfo> ships = new List<ShipInfo>();
    public string lastLogOutTime = "";
    public double secondsDoubleGain = 0;

    ////********** SETTINGS **********
    public bool isVolumeSFXOn = true;
    public bool isVolumeMusicOn = true;
    public bool isVibrationOn = true;
    public bool isPerformanceModeOn = true;

    ////********** DAILY REWARDS **********
    public List<int> listRewardsIndexes = new List<int>();
    public int currentRewardIndex = 0;
    public int cooldownSeconds = 0;

}
