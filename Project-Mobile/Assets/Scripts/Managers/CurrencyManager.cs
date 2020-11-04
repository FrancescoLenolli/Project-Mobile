using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void EventUpdateCurrencyText(long value);
public delegate void EventUpdateIdleGainText(int value);
public delegate void EventUpdateDoubleGainTimeText(int value);
public delegate void EventSpawnTextAtInputPosition(Vector3 position);
public delegate void EventBackgroundGainCalculated(int currency);

public class CurrencyManager : Singleton<CurrencyManager>
{
    public event EventUpdateCurrencyText EventUpdateCurrencyText;
    public event EventUpdateIdleGainText EventUpdateIdleGainText;
    public event EventUpdateDoubleGainTimeText EventUpdateDoubleGainTimeText;
    public event EventSpawnTextAtInputPosition EventSpawnTextAtInputPosition;
    public event EventBackgroundGainCalculated EventBackgroundGainCalculated;

    private GameManager gameManager = null;
    private int currentQuantityModifierIndex = 0;
    private int lastCurrencyIdleGain = 0;
    private int lastModifierIdleGain = 0;
    private int backgroundGain = 0;
    private bool isIdleGainDoubled = false;
    private int timeIdleGainDoubled = 0;

    public long currency = 0;
    [Space(10)]
    public int currencyIdleGain = 0;
    public int currencyActiveGain = 0;
    public int modifierIdleGain = 1;
    public int modifierActiveGain = 1;
    [Space(10)]
    [Min(1)]
    [Tooltip("When watching an Ad, gain this percentage of the current Currency")]
    public int adGainPercentage = 1;
    [Min(60)]
    [Tooltip("When watching an Ad, double the IdleGain by n Time expressed in seconds")]
    public int adDoubleGainTime = 1;
    [Space(10)]
    public List<int> listQuantityModifier = new List<int>();

    private new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        currency = gameManager.playerData.playerCurrency;
        lastCurrencyIdleGain = gameManager.playerData.lastCurrencyIdleGain;
        lastModifierIdleGain = gameManager.playerData.lastModifierIdleGain;

        StartCoroutine(UpdateCurrency());
    }

    private void Update()
    {
        // If player tap on the screen...
        if (Input.GetMouseButtonDown(0))
        {
            //... and if it's not tapping over a UI Object...
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //... Spawn object in the tap position, and add currency based on the active modifiers.
                EventSpawnTextAtInputPosition?.Invoke(Input.mousePosition);
                AddActiveCurrency();
            }
        }
    }


    // Add currency to the Player's current currency value.
    private void AddCurrency(long value, int modifier = 1)
    {
        // Currenct idle gain. Double it if the Player watches Ad that doubles IdleGain.
        currency += isIdleGainDoubled ? (value * modifier) * 2 : (value * modifier);

        // If idle gain is 0, there is no point in doing this operations.
        if (currency != 0)
        {
            if (currency >= long.MaxValue)
            {
                currency = long.MaxValue;
            }

            // Update UI to show the current Currency.
            EventUpdateCurrencyText?.Invoke(currency);
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

    // Start coroutine that doubles the amount of Idle currency gained.
    private void EnableDoubleIdleGain()
    {
        StartCoroutine(DoubleIdleGain());
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

    /// <summary>
    /// Add value to modifierIdleGain.
    /// </summary>
    /// <param name="value"></param>
    public void ChangeModifierIdleGain(int value)
    {
        modifierIdleGain += value;
    }

    /// <summary>
    /// Add value to modifierActiveGain.
    /// </summary>
    /// <param name="value"></param>
    public void ChangeModifierActiveGain(int value)
    {
        modifierActiveGain += value;
    }

    /// <summary>
    /// Change current quantity modifier.
    /// Determines how many ships will be bought next.
    /// </summary>
    /// <returns></returns>
    public int CycleModifierAndReturnValue()
    {
        currentQuantityModifierIndex++;
        if (currentQuantityModifierIndex > listQuantityModifier.Count - 1) currentQuantityModifierIndex = 0;

        return listQuantityModifier[currentQuantityModifierIndex];

    }

    /// <summary>
    /// Return the current quantity of ships that will be bought.
    /// </summary>
    /// <returns></returns>
    public int GetQuantityToBuy()
    {
        return listQuantityModifier[currentQuantityModifierIndex];
    }

    /// <summary>
    /// Add a percentage of the current Currency to the total Currency after watching an Ad.
    /// </summary>
    public void AddCurrencyAdvertisement()
    {
        long bonus = (currency * adGainPercentage) / 100;
        AddCurrency(bonus);
        Debug.Log($"Ad Watched, gained {bonus}");
    }

    /// <summary>
    /// Add time in seconds to timer that double Idle currency gained.
    /// Once the timer is at zero, Double Gain is disabled.
    /// </summary>
    /// <param name="moreTime"></param>
    public void AddDoubleIdleGainTime()
    {
        if (timeIdleGainDoubled == 0)
        {
            // if time is set to 0, start doubling IdleGain....
            timeIdleGainDoubled += adDoubleGainTime;
            EnableDoubleIdleGain();
        }
        else
        {
            //....if not, just add more time.
            timeIdleGainDoubled += adDoubleGainTime;
        }
    }

    /// <summary>
    ///  Use values from last game to calculate how much currency was gained since then. 
    /// </summary>
    /// <param name="seconds"></param>
    public void GetIdleGainSinceLastGame(int seconds)
    {
        backgroundGain = (lastCurrencyIdleGain * lastModifierIdleGain) * seconds;

        EventBackgroundGainCalculated?.Invoke(backgroundGain);
    }

    /// <summary>
    /// Add offline earnings to current Player's currency.
    /// </summary>
    /// <param name="collectionType"></param>
    public void AddOfflineEarnings(CanvasOfflineEarning.CollectionType collectionType)
    {
        // Player can simply collect offline earnings or double it by watching an ad.
        switch (collectionType)
        {
            case CanvasOfflineEarning.CollectionType.Normal:
                currency += backgroundGain;
                break;

            case CanvasOfflineEarning.CollectionType.DoubleAd:
                currency += backgroundGain * 2;
                break;

            default:
                Debug.LogWarning("Currency Manager : Something wrong when collecting Offline Earnings");
                break;
        }

        EventUpdateCurrencyText?.Invoke(currency);
    }

    // Add currency based on idle values every second.
    private IEnumerator UpdateCurrency()
    {
        while (currency < long.MaxValue)
        {
            AddIdleCurrency();

            EventUpdateIdleGainText?.Invoke(currencyIdleGain * modifierIdleGain);

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator DoubleIdleGain()
    {
        isIdleGainDoubled = true;

        while (timeIdleGainDoubled > 0)
        {
            yield return new WaitForSeconds(1);

            --timeIdleGainDoubled;

            EventUpdateDoubleGainTimeText?.Invoke(timeIdleGainDoubled);
        }

        isIdleGainDoubled = false;
    }
}
