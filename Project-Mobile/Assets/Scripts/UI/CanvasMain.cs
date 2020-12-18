using System;
using TMPro;
using UnityEngine;

public delegate void ShowOptionsPanel();
public class CanvasMain : MonoBehaviour
{
    public event ShowOptionsPanel EventShowOptionsPanel;

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
        textDoubleGainTime.text = "";

        EventShowOptionsPanel += FindObjectOfType<CanvasOptions>().MoveToPosition;
        currencyManager.EventUpdateTextCurrency += UpdateCurrencyText;
        currencyManager.EventUpdateTextIdleGain += UpdateIdleGainText;
        currencyManager.EventUpdateTextDoubleGainTime += UpdateDoubleGainTime;
        currencyManager.EventSendTouchPosition += InstantiateTapObject;
    }

    private void UpdateIdleGainText(double value)
    {
        textIdleGain.text =  value != 0 ? $"+{Formatter.FormatValue(value)}/s" : "";
    }

    public void UpdateCurrencyText(double value)
    {
        textCurrency.text = Formatter.FormatValue(value);
    }

    private void UpdateDoubleGainTime(int value)
    {
        textDoubleGainTime.text = value == 0 ? "" : $"x2 {TimeSpan.FromSeconds(value)}";
    }

    // When tapping on screen, instantiate object that displays the amount of currency gained by tapping.
    private void InstantiateTapObject(Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(currencyManager.currencyActiveGain * currencyManager.modifierActiveGain, currencyManager.spriteCurrency);
    }

    public void ShowOptionsPanel()
    {
        EventShowOptionsPanel?.Invoke();
    }
}
