using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Action EventSaveData;
    public Action EventInitData;
    public Action<TimeSpan> EventSendOfflineTime;

    private bool isFirstSession = true;
    private DateTime logInTime;
    private DateTime logOutTime;

    [Tooltip("If TRUE, you can buy ships at no cost.")]
    public bool isTesting = false;
    [Tooltip("If TRUE, reset data every time you start the game.")]
    public bool canResetData = false;
    [Space(10)]
    public AdsManager adsManager = null;

    private new void Awake()
    {
        base.Awake();
        LogIn();
        SaveManager.Load();
    }

    private void Start()
    {
        if(canResetData)
        {
            SaveManager.ResetData();
        }

        CurrencyManager currencyManager = CurrencyManager.Instance;
        ShipsManager shipsManager = FindObjectOfType<ShipsManager>();

        SubscribeToEventInitData(InitData);
        SubscribeToEventInitData(shipsManager.InitData);
        SubscribeToEventInitData(currencyManager.InitData);
        SubscribeToEventInitData(Settings.InitData);

        SubscribeToEventSaveData(SaveData);
        SubscribeToEventSaveData(shipsManager.SaveData);
        SubscribeToEventSaveData(currencyManager.SaveData);
        SubscribeToEventSaveData(Settings.SaveData);

        SubscribeToEventSendOfflineTime(currencyManager.CalculateOfflineGain);

        EventInitData?.Invoke();

        UnsubscribeToEventInitData(Settings.InitData);
        UnsubscribeToEventInitData(currencyManager.InitData);
        UnsubscribeToEventInitData(shipsManager.InitData);
        UnsubscribeToEventInitData(InitData);

        CalculateOfflineTime();
    }

    // pause == TRUE: the app is in the background.
    // pause == FALSE: the app is in focus.
    private void OnApplicationPause(bool pause)
    {
        if (!isTesting)
        {
            if (pause)
            {
                LogOut();
                Save();
            }
            else
            {
                LogIn();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (!isTesting)
        {
            LogOut();
            Save();
        }
    }

    public bool IsFirstSession()
    {
        return isFirstSession;
    }

    public void PrestigeUp()
    {
        PlayerData newData = new PlayerData();
        newData.prestigeLevel = ++SaveManager.PlayerData.prestigeLevel;
        newData.premiumCurrency = SaveManager.PlayerData.premiumCurrency;

        SaveManager.Save(newData);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


    public void SubscribeToEventInitData(Action method)
    {
        EventInitData += method;
    }
    public void UnsubscribeToEventInitData(Action method)
    {
        EventInitData -= method;
    }
    public void SubscribeToEventSaveData(Action method)
    {
        EventSaveData += method;
    }
    public void SubscribeToEventSendOfflineTime(Action<TimeSpan> method)
    {
        EventSendOfflineTime += method;
    }


    private void InitData()
    {
        string lastLogOut = SaveManager.PlayerData.lastLogOutTime;

        if (lastLogOut != "")
            logOutTime = Convert.ToDateTime(lastLogOut);
        else
            logOutTime = logInTime;
    }

    private void SaveData()
    {
        SaveManager.PlayerData.lastLogOutTime = logOutTime.ToString();
    }

    private void Save()
    {
        EventSaveData?.Invoke();
        SaveManager.Save();
    }

    private void LogIn()
    {
        logInTime = DateTime.Now;
    }

    private void LogOut()
    {
        logOutTime = DateTime.Now;
    }    

    private void CalculateOfflineTime()
    {
        TimeSpan timeOffline = logInTime.Subtract(logOutTime);
        EventSendOfflineTime?.Invoke(timeOffline);
    }
}
