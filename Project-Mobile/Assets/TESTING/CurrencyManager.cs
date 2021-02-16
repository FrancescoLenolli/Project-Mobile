using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private Action<double> EventSendCurrencyValue;
    private Action<double> EventSendPremiumCurrencyValue;
    private Action<double> EventSendPassiveCurrencyGainValue;
    private Action<double, Vector3> EventSendActiveCurrencyGainValue;

    public Sprite spriteCurrency;
    public Sprite spritePremiumCurrency;
    [Space]
    public double currency;
    public double premiumCurrency;
    public double activeCurrencyGain;
    public List<Ship> ships = new List<Ship>();

    public void Update()
    {
        if (IsPlayerTapping())
        {
            CollectActiveCurrency();
        }
    }

    public void InitData()
    {
        AddCurrency(SaveManager.GetData().currency);
        AddPassiveCurrency();
    }

    public void SaveData()
    {
        SaveManager.GetData().currency = currency;
    }

    public List<Ship> GetShips()
    {
        return ships;
    }

    public void AddShip(Ship ship)
    {
        ships.Add(ship);
    }

    public void AddCurrency(double value)
    {
        currency += value;
        EventSendCurrencyValue?.Invoke(currency);
        //Debug.Log(currency.ToString());
    }

    public void AddPremiumCurrency(double value)
    {
        premiumCurrency += value;
        EventSendPremiumCurrencyValue?.Invoke(premiumCurrency);
    }

    public void RemoveCurrency(double value)
    {

        currency -= value;
        EventSendCurrencyValue?.Invoke(currency);
        //Debug.Log(currency.ToString());
    }

    public void RemovePremiumCurrency(double value)
    {
        premiumCurrency -= value;
        EventSendPremiumCurrencyValue?.Invoke(premiumCurrency);
    }


    public void SubscribeToEventSendCurrencyValue(Action<double> method)
    {
        EventSendCurrencyValue += method;
    }

    public void SubscribeToEventSendPremiumCurrency(Action<double> method)
    {
        EventSendPremiumCurrencyValue += method;
    }

    public void SubscribeToEventSendPassiveCurrencyGainValue(Action<double> method)
    {
        EventSendPassiveCurrencyGainValue += method;
    }

    public void SubscribeToEventSendActiveCurrencyValue(Action<double, Vector3> method)
    {
        EventSendActiveCurrencyGainValue += method;
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

        foreach (Ship ship in ships)
        {
            currencyGain += ship.GetTotalCurrencyGain();
        }

        return currencyGain;
    }

    private double GetActiveCurrencyGain()
    {
        double activeGain = Math.Round(GetTotalPassiveCurrencyGain() / 3);
        if (activeGain == 0)
            activeGain = 5;

        return activeGain;
    }

    private void CollectActiveCurrency()
    {
        double activeGain = GetActiveCurrencyGain();
        AddCurrency(activeGain);
        EventSendActiveCurrencyGainValue?.Invoke(activeGain, Input.mousePosition);

        Debug.Log($"Collected {activeGain} by tapping");
    }

    private IEnumerator PassiveCurrencyGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            double totalPassiveCurrency = GetTotalPassiveCurrencyGain();
            EventSendPassiveCurrencyGainValue?.Invoke(totalPassiveCurrency);
            AddCurrency(totalPassiveCurrency);

            yield return null;
        }
    }
}
