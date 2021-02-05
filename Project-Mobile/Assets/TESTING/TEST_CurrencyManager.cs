using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CurrencyManager : MonoBehaviour
{
    private bool canTest = false;

    public double currency;
    public double premiumCurrency;
    public double activeCurrencyGain;
    public List<TEST_Ship> ships = new List<TEST_Ship>();

    private void Start()
    {
        AddPassiveCurrency();
    }

    public void Update()
    {
        if (IsPlayerTapping())
        {
            canTest = true;
        }
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
        foreach(TEST_Ship ship in ships)
        {
            currencyGain += ship.GetTotalCurrencyGain();
        }
        return currencyGain;
    }

    private IEnumerator PassiveCurrencyGain()
    {
        while(true)
        {
            if (canTest)
            {
                yield return new WaitForSeconds(1f);
                currency += GetTotalPassiveCurrencyGain();
                Debug.Log(currency.ToString());
                canTest = false;
            }
            yield return null;
        }
    }
}
