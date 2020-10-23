using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void EventUpdateCurrencyText(long value);
public delegate void EventSpawnTextAtInputPosition(Vector3 position);
public class CurrencyManager : Singleton<CurrencyManager>
{
    public event EventUpdateCurrencyText UpdateCurrencyText;
    public event EventSpawnTextAtInputPosition SpawnTextAtInputPosition;

    private GameManager gameManager = null;
    private int currentQuantityModifierIndex = 0;
    private int lastCurrencyIdleGain = 0;
    private int lastModifierIdleGain = 0;
    private EventSystem eventSystem;

    public long currency = 0;
    public int premiumCurrency = 0;
    public int currencyIdleGain = 0;
    public int currencyActiveGain = 0;
    public int modifierIdleGain = 1;
    public int modifierActiveGain = 1;
    public List<int> listQuantityModifier = new List<int>();

    private new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        eventSystem = EventSystem.current;
        gameManager = GameManager.Instance;

        currency = gameManager.playerData.playerCurrency;
        lastCurrencyIdleGain = gameManager.playerData.lastCurrencyIdleGain;
        lastModifierIdleGain = gameManager.playerData.lastModifierIdleGain;

        StartCoroutine(UpdateCurrency());
    }

    private void Update()
    {
        // If player tap on the screen...
        if(Input.GetMouseButtonDown(0))
        {
            //... and if it's not tapping over a UI Object...
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //... Spawn object in the tap position, and add currency based on the active modifiers.
                SpawnTextAtInputPosition?.Invoke(Input.mousePosition);
                AddActiveCurrency();
            }
        }
    }

    // Add currency to the current player currency.
    private void AddCurrency(long value, int modifier = 1)
    {
        // Currenct idle gain.
        currency += (value * modifier);

        // If idle gain is 0, there is no point in doing this operations.
        if (currency != 0)
        {
            if (currency >= long.MaxValue)
            {
                currency = long.MaxValue;
            }

            UpdateCurrencyText?.Invoke(currency);
        }
    }

    // Add currency based on the ActiveGain and relative modifiers.
    private void AddActiveCurrency()
    {
        AddCurrency(currencyActiveGain, modifierActiveGain);
    }

    // Add currency based on the IdleGain and relative modifiers.
    private void AddIdleCurrency()
    {
        AddCurrency(currencyIdleGain, modifierIdleGain);
    }

    /// <summary>
    /// Increase Currency Idle Gain by value. This determines how much currency is gained in background.
    /// </summary>
    /// <param name="value">How much CurrencyIdleGain is increased.</param>
    public void IncreaseCurrencyIdleGain(int value)
    {
        currencyIdleGain += value;
    }

    /// <summary>
    /// Decrease Currency Idle Gain by value. Used when recalculating production multiplier of a ship, after buying an upgrade.
    /// </summary>
    /// <param name="value"></param>
    public void DecreaseCurrencyIdleGain(int value)
    {
        currencyIdleGain -= value;
    }

    /// <summary>
    /// Increase Currency Active Gain by value. This determines how much currency is gained by tapping on the game view.
    /// </summary>
    /// <param name="value">How much the CurrencyActiveGain is increased.</param>
    public void ChangeCurrencyActiveGain(int value)
    {
        currencyActiveGain += value;
    }

    public void ChangeModifierIdleGain(int value)
    {
        modifierIdleGain += value;
    }

    public void ChangeModifierActiveGain(int value)
    {
        modifierActiveGain += value;
    }

    // Change current quantity modifier.
    public int CycleModifierAndReturnValue()
    {
        currentQuantityModifierIndex++;
        if (currentQuantityModifierIndex > listQuantityModifier.Count - 1) currentQuantityModifierIndex = 0;

        return listQuantityModifier[currentQuantityModifierIndex];

    }

    // Return the current Quantity of Ships that will be bought.
    // Ships can be bought one at a time, or more.
    public int ReturnModifierValue()
    {
        return listQuantityModifier[currentQuantityModifierIndex];
    }

    /************** Functions called from PanelExtra ***********/
    public void AddMoreCurrency(long currency)
    {
        AddCurrency(currency);
    }

    public void AddMorePremiumCurrency(int premiumCurrency)
    {
        Debug.Log("TODO: Premium Currency");
    }

    public void GetIdleGainSinceLastGame(int seconds)
    {
        StartCoroutine(CalculateIdleGainSinceLastGame(seconds));
    }

    public void MultiplyIdleGain(float multiplierTime)
    {
        StartCoroutine(MultiplyIdleGainFor(multiplierTime));
    }
    /**********************************************************/


    // Add currency based on idle values every second.
    IEnumerator UpdateCurrency()
    {
        while (currency < long.MaxValue)
        {
            AddIdleCurrency();
            Debug.Log(currencyIdleGain * modifierIdleGain);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator MultiplyIdleGainFor(float time)
    {
        modifierIdleGain *= 2;
        yield return new WaitForSeconds(time);
        modifierIdleGain /= 2;
    }

    IEnumerator CalculateIdleGainSinceLastGame(int seconds)
    {
        yield return new WaitForSeconds(2);

        int backgroundIdleGain = (lastCurrencyIdleGain * lastModifierIdleGain) * seconds;
        currency += backgroundIdleGain;

        UpdateCurrencyText?.Invoke(currency);

        //Debug.Log($"Background Idle Gain is {backgroundIdleGain}");

        yield return new WaitForEndOfFrame();
    }
}
