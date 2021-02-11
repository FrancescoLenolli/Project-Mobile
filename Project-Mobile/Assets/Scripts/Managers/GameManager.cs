using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void SendTimeFromLastGame(double seconds);
public class GameManager : Singleton<GameManager>
{
    public event SendTimeFromLastGame EventSendTimeFromLastGame;

    public Action EventSaveData;
    public Action EventInitData;

    // Time when the last session ended.
    private DateTime lastSessionTime = DateTime.Now;
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
        CalculateOfflineTime();
    }

    private void Start()
    {
        SaveManager.Load();
        CurrencyManager currencyManager = CurrencyManager.Instance;
        ShipsManager shipsManager = FindObjectOfType<ShipsManager>();

        SubscribeToEventInitData(shipsManager.InitData);
        SubscribeToEventInitData(currencyManager.InitData);

        SubscribeToEventSaveData(shipsManager.SaveData);
        SubscribeToEventSaveData(currencyManager.SaveData);

        EventInitData?.Invoke();

        UnsubscribeToEventInitData(shipsManager.InitData);
        UnsubscribeToEventInitData(currencyManager.InitData);

        //if (playerData.currencyIdleGain != 0)
        //    StartCoroutine(WaitToCalculateOfflineGain(3));
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //lastSessionTime = DateTime.Now;
            //SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        //lastSessionTime = DateTime.Now;
        //isFirstSession = false;
        SaveData();
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

    private void SaveData()
    {
        EventSaveData?.Invoke();
        SaveManager.Save();
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
}
