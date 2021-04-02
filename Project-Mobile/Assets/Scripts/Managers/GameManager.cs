﻿using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>, IDataHandler
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
        if (canResetData)
        {
            SaveManager.ResetData();
        }

        CurrencyManager currencyManager = CurrencyManager.Instance;
        DailyRewardsManager rewardsManager = FindObjectOfType<DailyRewardsManager>();
        List<Action> actionsInitData = new List<Action>();
        List<Action> actionsSaveData = new List<Action>();
        List<Action<TimeSpan>> actionsOfflineTime = new List<Action<TimeSpan>>
        {
            currencyManager.CalculateOfflineGain, rewardsManager.CalculateOfflineTime
        };

        IDataHandler[] dataHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IDataHandler>().ToArray();

        foreach(IDataHandler dataHandler in dataHandlers)
        {
            actionsInitData.Add(dataHandler.InitData);
            actionsSaveData.Add(dataHandler.SaveData);
        }

        actionsInitData.Add(Settings.InitData);
        actionsSaveData.Add(Settings.SaveData);

        Observer.AddObservers(ref EventInitData, actionsInitData);
        Observer.AddObservers(ref EventSaveData, actionsSaveData);
        Observer.AddObservers(ref EventSendOfflineTime , actionsOfflineTime);

        EventInitData?.Invoke();
        Observer.RemoveAllObservers(ref EventInitData);

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

    public void InitData()
    {
        string lastLogOut = SaveManager.PlayerData.lastLogOutTime;

        logOutTime = lastLogOut != "" ? Convert.ToDateTime(lastLogOut) : logInTime;
    }

    public void SaveData()
    {
        SaveManager.PlayerData.lastLogOutTime = logOutTime.ToString();
    }


    public bool IsFirstSession()
    {
        return isFirstSession;
    }

    public void Save()
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
