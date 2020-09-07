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

    public long currency = 0;
    public int currencyIdleGain = 0;
    public int currencyActiveGain = 0;
    public int modifierIdleGain = 1;
    public int modifierActiveGain = 1;
    public List<int> listQuantityModifier = new List<int>();
    [HideInInspector] public int currentModifierIndex = 0;

    private EventSystem eventSystem;

    private new void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        SaveManager.Instance.Load();
        currency = SaveManager.Instance.playerData.playerCurrency;
        StartCoroutine(UpdateCurrency());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                SpawnTextAtInputPosition?.Invoke(Input.mousePosition);
                AddCurrency(currencyActiveGain, modifierActiveGain);
            }
        }
    }

    private void AddCurrency(int value, int modifier)
    {
        currency += (value * modifier);

        if(currency >= long.MaxValue)
        {
            currency = long.MaxValue;
        }

        UpdateCurrencyText?.Invoke(currency);
        //Debug.Log($"Currency: {currency}");
    }

    public void AddActiveCurrency()
    {
        AddCurrency(currencyActiveGain, modifierActiveGain);
    }

    /// <summary>
    /// Increase Currency Idle Gain by value. This determines how much currency is gained in background.
    /// </summary>
    /// <param name="value">How much CurrencyIdleGain is increased.</param>
    public void ChangeCurrencyIdleGain(int value)
    {
        currencyIdleGain += value;
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

    public int CycleModifierAndReturnValue()
    {
        currentModifierIndex++;
        if (currentModifierIndex > listQuantityModifier.Count - 1) currentModifierIndex = 0;

        return listQuantityModifier[currentModifierIndex];

    }

    public int ReturnModifierValue()
    {
        return listQuantityModifier[currentModifierIndex];
    }


    IEnumerator UpdateCurrency()
    {
        while (currency < long.MaxValue)
        {
            AddCurrency(currencyIdleGain, modifierIdleGain);
            yield return new WaitForSeconds(1);
        }
    }
}
