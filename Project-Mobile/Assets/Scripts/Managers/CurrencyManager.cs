using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>, IDataHandler
{
    public double currency;
    public int premiumCurrency;
    public CurrencyData data;

    private IPassiveGainHandler passiveGainCalculator;
    private double secondsDoubleGain = 0;
    private List<Collectible> collectibles = new List<Collectible>();

    public Action<double> EventSendCurrencyValue;
    public Action<double> EventSendPassiveCurrencyGainValue;
    public Action<double> EventSendDoubleGainTime;
    public Action<TimeSpan, double> EventGainedOfflineCurrency;
    public Action<int> EventSendPremiumCurrencyValue;
    public Action<double, Vector3> EventSendActiveCurrencyGainValue;

    public List<Collectible> Collectibles { get => collectibles; }
    public double SecondsDoubleGain { get => secondsDoubleGain; set => secondsDoubleGain = value; }

    public void Update()
    {
        if (Utils.IsPlayerTapping())
        {
            TapBehaviour();
        }
    }

    public void InitData()
    {
        passiveGainCalculator = new PassiveGainCalculator();

        CanvasMain canvasMain = FindObjectOfType<CanvasMain>();
        CanvasOfflineEarning canvasOfflineEarning = FindObjectOfType<CanvasOfflineEarning>();

        canvasMain.InitData(this);

        secondsDoubleGain = SaveManager.PlayerData.secondsDoubleGain;
        AddCurrency(SaveManager.PlayerData.currency);
        AddPremiumCurrency(SaveManager.PlayerData.premiumCurrency);
        AddPassiveCurrency();

        Observer.AddObserver(ref EventGainedOfflineCurrency, canvasOfflineEarning.ShowPanel);

        EventSendPassiveCurrencyGainValue?.Invoke(GetTotalPassiveCurrencyGain());
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
            double offlineGain;
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

    private void AddPassiveCurrency()
    {
        StartCoroutine(PassiveCurrencyGain());
    }

    private int GetOfflineBonusPct()
    {
        int totalPct = data.prestigeOfflineBonusPct * PrestigeManager.prestigeLevel;

        return totalPct == 0 ? data.prestigeOfflineBonusPct / 2 : totalPct;
    }

    private double GetTotalPassiveCurrencyGain()
    {
        return passiveGainCalculator.GetTotalPassiveGain(this);
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
        if (UtilsUI.IsPointerOverUI())
            return;

        double activeGain = GetActiveCurrencyGain();
        AddCurrency(activeGain);
        EventSendActiveCurrencyGainValue?.Invoke(activeGain, Input.mousePosition);

        Vibration.VibrateSoft();
    }

    private IEnumerator PassiveCurrencyGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            double totalPassiveCurrency = GetTotalPassiveCurrencyGain();
            if (secondsDoubleGain > 0)
            {
                --secondsDoubleGain;
            }

            AddCurrency(totalPassiveCurrency);

            EventSendDoubleGainTime?.Invoke(secondsDoubleGain);
            EventSendPassiveCurrencyGainValue?.Invoke(totalPassiveCurrency);

            yield return null;
        }
    }
}
