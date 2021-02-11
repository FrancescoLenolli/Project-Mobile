using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private Action<double> EventSendCurrencyValue;
    private Action<double> EventSendPassiveCurrencyGainValue;

    public double currency;
    public double premiumCurrency;
    public double activeCurrencyGain;
    public List<Ship> ships = new List<Ship>();

    public void Update()
    {
        if (IsPlayerTapping())
        {
            //TODO: Active Currency gain
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

    public void RemoveCurrency(double value)
    {

        currency -= value;
        EventSendCurrencyValue?.Invoke(currency);
        //Debug.Log(currency.ToString());
    }

    public void SubscribeToEventSendCurrencyValue(Action<double> method)
    {
        EventSendCurrencyValue += method;
    }

    public void SubscribeToEventSendPassiveCurrencyGainValue(Action<double> method)
    {
        EventSendPassiveCurrencyGainValue += method;
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

        EventSendPassiveCurrencyGainValue?.Invoke(currencyGain);
        return currencyGain;
    }

    private IEnumerator PassiveCurrencyGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            AddCurrency(GetTotalPassiveCurrencyGain());

            yield return null;
        }
    }
}
