using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public delegate void SendTimeFromLastGame(int seconds);
public class GameManager : Singleton<GameManager>
{
    public SendTimeFromLastGame eventSendTimeFromLastGame;

    private DateTime lastPlayedTime;

    [HideInInspector] public PlayerData playerData = null;
    [HideInInspector] public string file = "PlayerData.json";

    public string playerName = "";
    [Min(0)]
    public long playerCurrency = 0;
    public bool isSFXVolumeOn = true;
    public bool isMusicVolumeOn = true;
    public bool isVibrationOn = true;

    [Header("DEBUG")]
    public bool canSave = true;

    private new void Awake()
    {
        base.Awake();

        // Load Saved data.
        playerData = Load();

        isSFXVolumeOn = playerData.SFXVolume;
        isMusicVolumeOn = playerData.MusicVolume;
        isVibrationOn = playerData.VibrationOn;

        lastPlayedTime = Convert.ToDateTime(playerData.lastPlayedTime);
    }

    private void Start()
    {
        eventSendTimeFromLastGame += CurrencyManager.Instance.GetIdleGainSinceLastGame;

        eventSendTimeFromLastGame?.Invoke(GetSecondsFromLastGame());
    }

    private void OnApplicationQuit()
    {
        lastPlayedTime = DateTime.Now;
        SaveCurrentData();
    }

    private int GetSecondsFromLastGame()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSpan = currentTime.Subtract(lastPlayedTime);
        int seconds = (int)timeSpan.TotalSeconds;
        return seconds;
    }

    #region Save System

    // Store new data in case something changed during the game.
    public void SaveCurrentData()
    {
        playerData.playerName = playerName;
        playerData.playerCurrency = CurrencyManager.Instance.currency;
        playerData.SFXVolume = isSFXVolumeOn;
        playerData.MusicVolume = isMusicVolumeOn;
        playerData.VibrationOn = isVibrationOn;
        //playerData.playerShips = ShipsManager.Instance.listShipInfos;
        playerData.lastCurrencyIdleGain = CurrencyManager.Instance.currencyIdleGain;
        playerData.lastModifierIdleGain = CurrencyManager.Instance.modifierIdleGain;
        playerData.lastPlayedTime = lastPlayedTime.ToString();
        Save();
    }

    // Doesn't seems like a good idea, but I'll leave it for now.
    // Maybe have a list directly in GameManager?
    public void SaveShipInfos(List<ShipInfo> newList)
    {
        playerData.playerShips = newList;
        Save();
    }

    public void SaveUpgradesUnlocked(List<UpgradeInfo> newList)
    {
        playerData.playerUpgrades = newList;
        Save();
    }

    // Convert data to JSON, then save it.
    public void Save()
    {
        if (canSave)
        {
            string json = JsonUtility.ToJson(playerData);
            WriteToFile(file, json);
        }
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
            data = new PlayerData { };
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
    #endregion
}
