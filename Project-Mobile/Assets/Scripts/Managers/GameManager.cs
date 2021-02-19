using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action EventSaveData;
    public Action EventInitData;
    public Action<TimeSpan> EventSendOfflineTime;

    private bool isFirstSession = true;
    private DateTime logInTime;
    private DateTime logOutTime;

    [HideInInspector] public PlayerData playerData = null;
    [HideInInspector] public string file = "PlayerData.json";

    public bool isTesting = false;
    public bool canResetData = false;
    [Space]
    public string playerName = "";
    public bool isVolumeSFXOn = true;
    public bool isVolumeMusicOn = true;
    public bool isVibrationOn = true;
    [Space(10)]
    public AdsManager adsManager = null;

    private new void Awake()
    {
        base.Awake();
        LogIn();
    }

    private void Start()
    {
        SaveManager.Load();

        if(canResetData)
        {
            SaveManager.ResetData();
        }

        CurrencyManager currencyManager = CurrencyManager.Instance;
        ShipsManager shipsManager = FindObjectOfType<ShipsManager>();

        SubscribeToEventInitData(InitData);
        SubscribeToEventInitData(shipsManager.InitData);
        SubscribeToEventInitData(currencyManager.InitData);

        SubscribeToEventSaveData(SaveData);
        SubscribeToEventSaveData(shipsManager.SaveData);
        SubscribeToEventSaveData(currencyManager.SaveData);

        SubscribeToEventSendOfflineTime(currencyManager.CalculateOfflineGain);

        EventInitData?.Invoke();

        UnsubscribeToEventInitData(InitData);
        UnsubscribeToEventInitData(shipsManager.InitData);
        UnsubscribeToEventInitData(currencyManager.InitData);

        StartCoroutine(WaitToCalculateOfflineGain(3));
    }

    private void OnApplicationPause(bool pause)
    {
        if (!isTesting)
        {
            if (pause)
            {
                //lastSessionTime = DateTime.Now;
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
        //lastSessionTime = DateTime.Now;
        //isFirstSession = false;
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
        string lastLogOut = SaveManager.GetData().lastLogOutTime;

        if (lastLogOut != "")
            logOutTime = Convert.ToDateTime(lastLogOut);
        else
            logOutTime = logInTime;
    }

    private void SaveData()
    {
        SaveManager.GetData().lastLogOutTime = logOutTime.ToString();
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

    private IEnumerator WaitToCalculateOfflineGain(float waitTime)
    {
        TimeSpan timeOffline = logInTime.Subtract(logOutTime);

        yield return new WaitForSeconds(waitTime);
        EventSendOfflineTime?.Invoke(timeOffline);

        yield return null;
    }
}
