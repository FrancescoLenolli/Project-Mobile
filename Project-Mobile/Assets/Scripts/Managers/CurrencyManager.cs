using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private Action<double> EventSendCurrencyValue;
    private Action<double> EventSendPassiveCurrencyGainValue;
    private Action<double> EventSendDoubleGainTime;
    private Action<TimeSpan, double> EventGainedOfflineCurrency;
    private Action<int> EventSendPremiumCurrencyValue;
    private Action<double, Vector3> EventSendActiveCurrencyGainValue;

    private double secondsDoubleGain = 0;
    private List<Collectible> collectibles = new List<Collectible>();

    public double currency;
    public int premiumCurrency;
    public CurrencyData data;

    public void Update()
    {
        if (IsPlayerTapping())
        {
            TapBehaviour();
        }
    }

    public void InitData()
    {
        CanvasMain canvasMain = FindObjectOfType<CanvasMain>();
        CanvasOfflineEarning canvasOfflineEarning = FindObjectOfType<CanvasOfflineEarning>();

        canvasMain.InitData();

        secondsDoubleGain = SaveManager.PlayerData.secondsDoubleGain;
        AddCurrency(SaveManager.PlayerData.currency);
        AddPremiumCurrency(SaveManager.PlayerData.premiumCurrency);
        AddPassiveCurrency();

        SubscribeToEventGainedPassiveCurrency(canvasOfflineEarning.ShowPanel);
    }

    public void SaveData()
    {
        SaveManager.PlayerData.currency = currency;
        SaveManager.PlayerData.premiumCurrency = premiumCurrency;
        SaveManager.PlayerData.secondsDoubleGain = secondsDoubleGain;
    }

    public void AddCollectible(Collectible collectible)
    {
        collectibles.Add(collectible);
    }

    public void AddCurrency(double value)
    {
        currency += value;
        EventSendCurrencyValue?.Invoke(currency);
    }

    public void AddPremiumCurrency(int value)
    {
        premiumCurrency += value;
        EventSendPremiumCurrencyValue?.Invoke(premiumCurrency);
    }

    public void RemoveCurrency(double value)
    {
        currency -= value;
        EventSendCurrencyValue?.Invoke(currency);
    }

    public void RemovePremiumCurrency(int value)
    {
        premiumCurrency -= value;
        EventSendPremiumCurrencyValue?.Invoke(premiumCurrency);
    }

    public void CalculateOfflineGain(TimeSpan timeOffline)
    {
        double secondsOffline = timeOffline.TotalSeconds;
        double totalOfflineGain;

        if (secondsOffline >= secondsDoubleGain)
        {
            double baseSecondsOffline = secondsOffline - secondsDoubleGain;
            double offlineGain = 0;
            double doubledOfflineGain = 0;

            offlineGain = MathUtils.Pct(GetOfflineBonusPct(), GetTotalPassiveCurrencyGain()) * baseSecondsOffline;
            if (secondsDoubleGain > 0)
                doubledOfflineGain = MathUtils.Pct(GetOfflineBonusPct(), GetTotalPassiveCurrencyGain()) * secondsDoubleGain * 2;

            totalOfflineGain = offlineGain + doubledOfflineGain;
            secondsDoubleGain = 0;
        }
        else
        {
            secondsDoubleGain -= secondsOffline;
            totalOfflineGain = MathUtils.Pct(GetOfflineBonusPct(), GetTotalPassiveCurrencyGain()) * secondsOffline * 2;
        }

        if (totalOfflineGain > 0)
        {
            EventGainedOfflineCurrency?.Invoke(timeOffline, totalOfflineGain);
        }
    }

    public void AddCurrencyFixedValue()
    {
        double value = MathUtils.Pct(data.adPctGain, currency);
        AddCurrency(value);
    }
    public bool BuyCurrencyFixedValue()
    {
        if (premiumCurrency >= data.extrasPremiumCost)
        {
            premiumCurrency -= data.extrasPremiumCost;
            AddCurrencyFixedValue();
            return true;
        }
        return false;
    }


    public void AddDoubleGainTime()
    {
        secondsDoubleGain += data.adHoursDoubleGain * 3600;
    }
    public bool BuyDoubleGainTime()
    {
        if (premiumCurrency >= data.extrasPremiumCost)
        {
            premiumCurrency -= data.extrasPremiumCost;
            AddDoubleGainTime();
            return true;
        }
        return false;
    }

    public void AddPremiumCurrencyFixedValue()
    {
        AddPremiumCurrency(data.adPremiumCurrencyGain);
    }

    public bool CanBuyWithPremium()
    {
        return data.extrasPremiumCost <= premiumCurrency;
    }


    public void SubscribeToEventSendCurrency(Action<double> method)
    {
        EventSendCurrencyValue += method;
    }
    public void SubscribeToEventSendPremiumCurrency(Action<int> method)
    {
        EventSendPremiumCurrencyValue += method;
    }
    public void SubscribeToEventSendPassiveCurrencyGain(Action<double> method)
    {
        EventSendPassiveCurrencyGainValue += method;
    }
    public void SubscribeToEventSendActiveCurrencyGain(Action<double, Vector3> method)
    {
        EventSendActiveCurrencyGainValue += method;
    }
    public void SubscribeToEventGainedPassiveCurrency(Action<TimeSpan, double> method)
    {
        EventGainedOfflineCurrency += method;
    }
    public void SubscribeToEventSendDoubleGainTime(Action<double> method)
    {
        EventSendDoubleGainTime += method;
    }


    private void AddPassiveCurrency()
    {
        StartCoroutine(PassiveCurrencyGain());
    }

    private bool IsPlayerTapping()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0);
    }

    private int GetOfflineBonusPct()
    {
        int totalPct = data.prestigeOfflineBonusPct * SaveManager.PlayerData.prestigeLevel;

        return totalPct == 0 ? data.prestigeOfflineBonusPct / 2 : totalPct;
    }

    private double GetTotalPassiveCurrencyGain()
    {
        double collectiblesGain = collectibles.Sum(x => x.TotalCurrencyGain);
        double prestigeBonusGain = 0;

        if (SaveManager.PlayerData.prestigeLevel != 0) // TODO: Prestige variable in GameManager
            prestigeBonusGain = MathUtils.Pct(data.basePassiveGainPercentage * SaveManager.PlayerData.prestigeLevel, collectiblesGain);

        return collectiblesGain + prestigeBonusGain;
    }

    private double GetActiveCurrencyGain()
    {
        double activeGain = MathUtils.Pct(GetTotalActiveGainPercentage(), GetTotalPassiveCurrencyGain());
        if (activeGain == 0)
            activeGain = data.baseActiveGainValue;

        return activeGain;
    }

    private double GetTotalActiveGainPercentage()
    {
        return data.baseActiveGainPercentage; // TODO: Upgrades that increase the active gain percentage
    }

    private void TapBehaviour()
    {
        // passing a value of 0 makes it work on mobile but not on pc, so I have to use both
        if (EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            double activeGain = GetActiveCurrencyGain();
            AddCurrency(activeGain);
            EventSendActiveCurrencyGainValue?.Invoke(activeGain, Input.mousePosition);

            Vibration.VibrateSoft();
        }
    }

    private IEnumerator PassiveCurrencyGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            double totalPassiveCurrency = GetTotalPassiveCurrencyGain();
            if (secondsDoubleGain > 0)
            {
                totalPassiveCurrency *= 2;
                secondsDoubleGain = secondsDoubleGain < 0 ? 0 : --secondsDoubleGain;
            }

            AddCurrency(totalPassiveCurrency);

            EventSendDoubleGainTime?.Invoke(secondsDoubleGain);
            EventSendPassiveCurrencyGainValue?.Invoke(totalPassiveCurrency);

            yield return null;
        }
    }
}
