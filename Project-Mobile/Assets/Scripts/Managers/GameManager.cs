﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void SendTimeFromLastGame(int seconds);
public class GameManager : Singleton<GameManager>
{
    public SendTimeFromLastGame eventSendTimeFromLastGame;

    private DateTime lastPlayedTime;
    private bool isResetting = false;

    [HideInInspector] public PlayerData playerData = null;
    [HideInInspector] public string file = "PlayerData.json";

    public string playerName = "";
    [Min(0)]
    public long playerCurrency = 0;
    public bool isVolumeSFXOn = true;
    public bool isVolumeMusicOn = true;
    public bool isVibrationOn = true;
    [Space(10)]
    public AdsManager adsManager = null;

    [HideInInspector] public bool canSaveData = true;
    [HideInInspector] public bool canDebug = false;


    private new void Awake()
    {
        base.Awake();

        // Load Saved data.
        playerData = Load();

        canSaveData = playerData.canSaveData;
        canDebug = playerData.canDebug;
        isVolumeSFXOn = playerData.SFXVolume;
        isVolumeMusicOn = playerData.MusicVolume;
        isVibrationOn = playerData.VibrationOn;

        if (playerData.lastPlayedTime != "")
        {
            lastPlayedTime = Convert.ToDateTime(playerData.lastPlayedTime);
        }
    }

    private void Start()
    {
        eventSendTimeFromLastGame += CurrencyManager.Instance.GetIdleGainSinceLastGame;

        StartCoroutine(WaitToCalculateOfflineGain(3));
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            lastPlayedTime = DateTime.Now;
            SaveCurrentData();
        }
    }

    private void OnApplicationQuit()
    {
        if (!isResetting)
        {
            lastPlayedTime = DateTime.Now;
            SaveCurrentData();
        }
    }

    // Calculate in seconds how much time passed from last session.
    // Used to calculate how much currency was gained offline.
    private int GetSecondsFromLastGame()
    {
        int seconds = 0;
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSpan = currentTime.Subtract(lastPlayedTime);
        seconds = (int)timeSpan.TotalSeconds;
        return seconds;
    }

    public void EnableSaveData()
    {
        canSaveData = !canSaveData;
        SaveCanSaveData();

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
        int secondsFromLastGame = GetSecondsFromLastGame();

        yield return new WaitForSeconds(waitTime);
        eventSendTimeFromLastGame?.Invoke(secondsFromLastGame);

        yield return null;
    }


    /***********  SAVE SYSTEM  ***********/

    // Store new data in case something changed during the game.
    public void SaveCurrentData()
    {
        playerData.playerName = playerName;
        playerData.playerCurrency = CurrencyManager.Instance.currency;
        playerData.SFXVolume = isVolumeSFXOn;
        playerData.MusicVolume = isVolumeMusicOn;
        playerData.VibrationOn = isVibrationOn;
        //playerData.playerShips = ShipsManager.Instance.listShipInfos;
        playerData.lastCurrencyIdleGain = CurrencyManager.Instance.currencyIdleGain;
        playerData.lastModifierIdleGain = CurrencyManager.Instance.modifierIdleGain;
        playerData.lastPlayedTime = lastPlayedTime.ToString();
        playerData.canDebug = canDebug;
        Save();
    }

    // DEBUG ONLY. Reset all progress and close application.
    public void ResetData()
    {
        isResetting = true;
        playerData = new PlayerData();
        Save();
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // DEBUG ONLY.
    private void SaveCanSaveData()
    {
        playerData.canSaveData = canSaveData;
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
        playerData.playerUpgradesUnlocked = newList;
        Save();
    }

    public void SaveUpgradesBought(List<UpgradeInfo> newList)
    {
        playerData.playerUpgradesBought = newList;
        Save();
    }

    // Convert data to JSON, then save it.
    public void Save()
    {
        if (canSaveData)
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
