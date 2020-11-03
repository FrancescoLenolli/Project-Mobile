using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CanvasMain : MonoBehaviour
{
    CurrencyManager currencyManager;

    public TextMeshProUGUI textCurrency;
    public TextMeshProUGUI textIdleGain;
    public TextMeshProUGUI textDoubleGainTime;
    public TapObject prefabTextMousePosition;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Start()
    {
        textCurrency.text = currencyManager.currency.ToString();

        currencyManager.EventUpdateCurrencyText += UpdateCurrencyText;
        currencyManager.EventUpdateIdleGainText += UpdateIdleGainText;
        currencyManager.EventUpdateDoubleGainTimeText += UpdateDoubleGainTime;
        currencyManager.EventSpawnTextAtInputPosition += InstantiatePrefab;
    }

    private void UpdateCurrencyText(long value)
    {
        textCurrency.text = value.ToString(); 
    }

    private void UpdateIdleGainText(int value)
    {
        textIdleGain.text = $"+ {value}/s";
    }

    private void UpdateDoubleGainTime(int value)
    {
        textDoubleGainTime.text = value == 0 ? "" : $"x2 {TimeSpan.FromSeconds(value)}";
    }

    private void InstantiatePrefab(Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(currencyManager.currencyActiveGain * currencyManager.modifierActiveGain);
    }
}
