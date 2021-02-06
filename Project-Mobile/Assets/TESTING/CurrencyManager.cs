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

    private void Start()
    {
        CanvasMain canvasMain = FindObjectOfType<CanvasMain>();

        SubscribeToEventSendCurrencyValue(canvasMain.UpdateCurrencyText);
        SubscribeToEventSendPassiveCurrencyGainValue(canvasMain.UpdatePassiveGainText);

        AddPassiveCurrency();
    }

    public void Update()
    {
        if (IsPlayerTapping())
        {
            //TODO: Active Currency gain
        }
    }

    public List<Ship> GetShips()
    {
        return ships;
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

            currency += GetTotalPassiveCurrencyGain();
            EventSendCurrencyValue?.Invoke(currency);
            //Debug.Log(currency.ToString());

            yield return null;
        }
    }
}
