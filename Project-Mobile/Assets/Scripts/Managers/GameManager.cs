using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void SendTimeFromLastGame(long seconds);
public delegate void InitialiseData();
public delegate void Save();
public class GameManager : Singleton<GameManager>
{
    public event SendTimeFromLastGame EventSendTimeFromLastGame;
    public event InitialiseData EventInitData;
    public event Save EventSaveData;

    // Time when the last session ended.
    private DateTime lastSessionTime;
    // Time when current session started.
    private DateTime currentSessionTime;
    // seconds passed from last session to current session.
    private int secondsOffline;
    private bool isFirstSession = true;

    [HideInInspector] public PlayerData playerData = null;
    [HideInInspector] public string file = "PlayerData.json";
    [HideInInspector] public TimeSpan timeOffline = TimeSpan.Zero;

    public string playerName = "";
    public bool isVolumeSFXOn = true;
    public bool isVolumeMusicOn = true;
    public bool isVibrationOn = true;
    [Space(10)]
    public AdsManager adsManager = null;

    private new void Awake()
    {
        base.Awake();

        // Load Saved data.
        playerData = Load();

        isFirstSession = playerData.isFirstSession;
        isVolumeSFXOn = playerData.SFXVolumeOn;
        isVolumeMusicOn = playerData.MusicVolumeOn;
        isVibrationOn = playerData.VibrationOn;

        if (playerData.lastPlayedTime != "")
        {
            lastSessionTime = Convert.ToDateTime(playerData.lastPlayedTime);
        }

        CalculateOfflineTime();
    }

    private void Start()
    {
        PanelShips panelShips = FindObjectOfType<PanelShips>();
        CanvasDailyRewards canvasDailyRewards = FindObjectOfType<CanvasDailyRewards>();
        CurrencyManager currencyManager = CurrencyManager.Instance;
        ShipsView shipsView = FindObjectOfType<ShipsView>();

        EventInitData += panelShips.InitShips;
        EventInitData += canvasDailyRewards.InitRewards;
        EventInitData += shipsView.InitData;
        EventSendTimeFromLastGame += currencyManager.GetIdleGainSinceLastGame;
        EventSaveData += SaveCurrentData;
        EventSaveData += panelShips.SaveShipsInfo;
        EventSaveData += FindObjectOfType<PanelShipsUpgrades>().SaveUpgradesInfo;
        EventSaveData += canvasDailyRewards.SaveRewardsData;
        EventSaveData += currencyManager.SaveCurrencyData;
        EventSaveData += shipsView.SaveData;

        EventInitData?.Invoke();

        if (playerData.lastCurrencyIdleGain != 0)
            StartCoroutine(WaitToCalculateOfflineGain(3));
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            lastSessionTime = DateTime.Now;
            SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        lastSessionTime = DateTime.Now;
        isFirstSession = false;
        SaveData();
    }

    /// <summary>
    /// Calculate how much time has passed since last session.
    /// </summary>
    private void CalculateOfflineTime()
    {
        if (lastSessionTime != DateTime.MinValue)
        {
            currentSessionTime = DateTime.Now;
            timeOffline = currentSessionTime.Subtract(lastSessionTime);

            secondsOffline = (int)timeOffline.TotalSeconds;
        }
    }

    /// <summary>
    /// Return true if this is the first time the game is being played.
    /// </summary>
    /// <returns></returns>
    public bool IsFirstSession()
    {
        return isFirstSession;
    }

    /// <summary>
    /// Return time from last session to the current one in seconds.
    /// </summary>
    /// <returns></returns>
    public long GetOfflineTime()
    {
        return secondsOffline;
    }

    /// <summary>
    /// Return true if 24 hours or more have passed since last game session.
    /// </summary>
    /// <returns></returns>
    public bool HasDayPassed()
    {
        return timeOffline.TotalDays >= 1;
    }

    public void SetVolumeSFX(bool isOn)
    {
        isVolumeSFXOn = isOn;
    }

    public void SetVolumeMusic(bool isOn)
    {
        isVolumeMusicOn = isOn;
    }

    public void SetVibration(bool isOn)
    {
        isVibrationOn = isOn;
    }

    private IEnumerator WaitToCalculateOfflineGain(float waitTime)
    {
        long secondsOffline = GetOfflineTime();

        yield return new WaitForSeconds(waitTime);
        EventSendTimeFromLastGame?.Invoke(secondsOffline);

        yield return null;
    }


    /***********  SAVE SYSTEM  ***********/

    private void SaveData()
    {
        EventSaveData?.Invoke();
        Save();
    }

    // Store new data in case something changed during the game.
    public void SaveCurrentData()
    {
        playerData.isFirstSession = isFirstSession;
        playerData.playerName = playerName;
        playerData.SFXVolumeOn = isVolumeSFXOn;
        playerData.MusicVolumeOn = isVolumeMusicOn;
        playerData.VibrationOn = isVibrationOn;
        playerData.lastPlayedTime = lastSessionTime.ToString();
    }

    // Convert data to JSON, then save it.
    public void Save()
    {
        string json = JsonUtility.ToJson(playerData);
        WriteToFile(file, json);
    }

    /// Load Saved Data, create new Data if none is found.
    public PlayerData Load()
    {
        PlayerData data;

        // Try to get Data from persistentDataPath...
        try
        {
            string path = Application.persistentDataPath + "/" + file;
            string _json = File.ReadAllText(path);
            data = JsonUtility.FromJson<PlayerData>(_json);
        }
        // ...if no File is found, create a new one.
        catch (FileNotFoundException)
        {
            data = new PlayerData();
        }

        return data;
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else Debug.Log("File not found!");
        return "";
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    /*********************************************************+*/
}
