using System;
using TMPro;
using UnityEngine;

public class CanvasMain : MonoBehaviour
{
    private CurrencyManager currencyManager;

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
        currencyManager.EventSpawnTextAtInputPosition += InstantiateTapObject;
    }

    private void UpdateIdleGainText(int value)
    {
        textIdleGain.text = $"+ {value}/s";
    }

    private void UpdateDoubleGainTime(int value)
    {
        textDoubleGainTime.text = value == 0 ? "" : $"x2 {TimeSpan.FromSeconds(value)}";
    }

    // When tapping on screen, instantiate object that displays the amount of currency gained by tapping.
    private void InstantiateTapObject(Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(currencyManager.currencyActiveGain * currencyManager.modifierActiveGain);
    }

    public void UpdateCurrencyText(long value)
    {
        textCurrency.text = value.ToString();
    }
}
