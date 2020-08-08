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

    private EventSystem eventSystem;

    private new void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        StartCoroutine(UpdateCurrency());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                SpawnTextAtInputPosition?.Invoke(Input.mousePosition);
                AddCurrency(currencyActiveGain + 666, modifierActiveGain);
        }
    }

    private void AddCurrency(int value, int modifier)
    {
        currency += (value * modifier);
        UpdateCurrencyText?.Invoke(currency);
        //Debug.Log($"Currency: {currency}");
    }

    public void AddActiveCurrency()
    {
        AddCurrency(currencyActiveGain, modifierActiveGain);
    }

    public void ChangeCurrencyIdleGain(int value)
    {
        currencyIdleGain += value;
    }

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



    IEnumerator UpdateCurrency()
    {
        while (currency < long.MaxValue)
        {
            AddCurrency(currencyIdleGain, modifierIdleGain);
            yield return new WaitForSeconds(1);
        }
    }
}
