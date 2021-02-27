using System;
using System.Collections;
using System.Collections.Generic;
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

        secondsDoubleGain = SaveManager.GetData().secondsDoubleGain;
        AddCurrency(SaveManager.GetData().currency);
        AddPremiumCurrency(SaveManager.GetData().premiumCurrency);
        AddPassiveCurrency();

        SubscribeToEventGainedPassiveCurrency(canvasOfflineEarning.ShowPanel);
    }

    public void SaveData()
    {
        SaveManager.GetData().currency = currency;
        SaveManager.GetData().premiumCurrency = premiumCurrency;
        SaveManager.GetData().secondsDoubleGain = secondsDoubleGain;
    }

    public List<Collectible> GetCollectibles()
    {
        return collectibles;
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
            double currencyGainedOffline = (GetTotalPassiveCurrencyGain() * baseSecondsOffline) / 3;
            double currencyDoubledOffline = ((GetTotalPassiveCurrencyGain() * secondsDoubleGain) / 3) * 2;

            totalOfflineGain = currencyGainedOffline + currencyDoubledOffline;
            secondsDoubleGain = 0;
        }
        else
        {
            secondsDoubleGain -= secondsOffline;
            totalOfflineGain = ((GetTotalPassiveCurrencyGain() * secondsDoubleGain) / 3) * 2;
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

    private double GetTotalPassiveCurrencyGain()
    {
        double currencyGain = 0f;

        foreach (Ship ship in collectibles)
        {
            currencyGain += ship.GetTotalCurrencyGain();
        }

        return currencyGain;
    }

    private double GetActiveCurrencyGain()
    {
        double activeGain = MathUtils.Pct(GetTotalActiveGainPercentage(), GetTotalPassiveCurrencyGain());
        if (activeGain == 0)
            activeGain = data.baseActiveGainValue;

        return activeGain;
    }

    private int GetTotalActiveGainPercentage()
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
