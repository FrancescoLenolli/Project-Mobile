using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void EventUpdateTextCurrency(double value);
public delegate void EventUpdateTextIdleGain(double value);
public delegate void EventUpdateTextDoubleGainTime(int value);
public delegate void EventSendTouchPosition(Vector3 position);
public delegate void EventSendBackgroundGainValue(double currency);

public class CurrencyManager : Singleton<CurrencyManager>
{
    public event EventUpdateTextCurrency EventUpdateTextCurrency;
    public event EventUpdateTextIdleGain EventUpdateTextIdleGain;
    public event EventUpdateTextDoubleGainTime EventUpdateTextDoubleGainTime;
    public event EventSendTouchPosition EventSendTouchPosition;
    public event EventSendBackgroundGainValue EventSendBackgroundGainValue;

    private GameManager gameManager = null;
    private int currentQuantityModifierIndex = 0;
    private double offlineEarnings = 0;
    private int timeDoubledIdleGain = 0;
    private bool isIdleGainDoubled = false;
    private double lastCurrencyIdleGain = 0;
    private float lastModifierIdleGain = 0;

    public double currency = 0;
    [Space(10)]
    public double currencyIdleGain = 0;
    public double currencyActiveGain = 0;
    public float modifierIdleGain = 1;
    public float modifierActiveGain = 1;
    public List<int> listQuantityModifier = new List<int>();
    [Space(10)]
    [Header("Ads Values")]
    [Min(1)]
    [Tooltip("When watching an Ad, gain this percentage of the current Currency")]
    public int adGainPercentage = 1;
    [Min(60)]
    [Tooltip("When watching an Ad, double the IdleGain by n Time expressed in seconds")]
    public int adDoubleGainTime = 1;
    [Space(10)]
    public Sprite spriteCurrency = null;

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
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            PlayerTapBehaviour();
        }
    }

    // What happens if the Player is tapping/clicking the screen.
    private void PlayerTapBehaviour()
    {
        // If the Player is tapping on a UI Element, do nothing...
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            //... else, Send tap position and add Active Currency.
            EventSendTouchPosition?.Invoke(Input.mousePosition);
            AddActiveCurrency();
        }
    }

    // Add currency to the Player's current currency value.
    private void AddCurrency(double value, float modifier = 1)
    {
        if (value != 0 || modifier != 0)
        {
            // Currenct idle gain. Double it if the double gain modifier is active.
            double newValue = isIdleGainDoubled ? (value * modifier) * 2 : (value * modifier);

            currency += newValue;
            EventUpdateTextCurrency?.Invoke(currency);
        }
        else
        {
            Debug.LogError("Something went wrong when Adding Currency.\nCheck if the value gained or the gain modifier have not a value of 0.");
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
    public void IncreaseCurrencyIdleGain(double value)
    {
        currencyIdleGain += value;
    }

    /// <summary>
    /// Decrease Currency Idle Gain by value. Used when recalculating production multiplier of a ship, after buying an upgrade.
    /// </summary>
    /// <param name="value"></param>
    public void DecreaseCurrencyIdleGain(double value)
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
    /// Return the current quantity of ships that will be bought next.
    /// </summary>
    /// <returns></returns>
    public int GetShipQuantityToBuy()
    {
        return listQuantityModifier[currentQuantityModifierIndex];
    }

    /// <summary>
    /// Add a percentage of the Total Currency to it after watching an Ad.
    /// </summary>
    public void AddCurrencyAdvertisement()
    {
        double bonus = (currency * adGainPercentage) / 100;
        AddCurrency(bonus);
        Debug.Log($"Ad Watched, gained {bonus}");
    }

    /// <summary>
    /// Add time in seconds to timer that double Idle currency gained.
    /// Once the timer is at zero, Double Gain is disabled.
    /// </summary>
    public void AddDoubleIdleGainTime()
    {
        if (timeDoubledIdleGain == 0)
        {
            // if time is set to 0, start doubling IdleGain....
            timeDoubledIdleGain += adDoubleGainTime;
            EnableDoubleIdleGain();
        }
        else
        {
            //....if not, just add more time.
            timeDoubledIdleGain += adDoubleGainTime;
        }
    }

    /// <summary>
    ///  Use values from last game to calculate how much currency was gained since then. 
    /// </summary>
    /// <param name="seconds"></param>
    public void GetIdleGainSinceLastGame(double seconds)
    {
        offlineEarnings = (lastCurrencyIdleGain * lastModifierIdleGain) * seconds;

        EventSendBackgroundGainValue?.Invoke(offlineEarnings);
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
                currency += offlineEarnings;
                break;

            case CanvasOfflineEarning.CollectionType.DoubleAd:
                currency += offlineEarnings * 2;
                break;

            default:
                Debug.LogWarning("Currency Manager : Something wrong when collecting Offline Earnings.");
                break;
        }

        EventUpdateTextCurrency?.Invoke(currency);
    }

    public void SaveCurrencyData()
    {
        gameManager.playerData.playerCurrency = currency;
        gameManager.playerData.lastCurrencyIdleGain = currencyIdleGain;
        gameManager.playerData.lastModifierIdleGain = modifierIdleGain;
    }

    // Add currency based on idle values every second.
    private IEnumerator UpdateCurrency()
    {
        while (currency < long.MaxValue)
        {
            AddIdleCurrency();

            EventUpdateTextIdleGain?.Invoke(currencyIdleGain * modifierIdleGain);

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator DoubleIdleGain()
    {
        isIdleGainDoubled = true;

        while (timeDoubledIdleGain > 0)
        {
            yield return new WaitForSeconds(1);

            --timeDoubledIdleGain;

            EventUpdateTextDoubleGainTime?.Invoke(timeDoubledIdleGain);
        }

        isIdleGainDoubled = false;
    }
}
