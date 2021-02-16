using System;
using TMPro;
using UnityEngine;

public delegate void ShowOptionsPanel();
public class CanvasMain : MonoBehaviour
{
    private Action EventShowOptionsPanel;

    private CurrencyManager currencyManager;

    public TextMeshProUGUI textCurrency;
    public TextMeshProUGUI textPassiveGain;
    public TextMeshProUGUI textDoubleGainTime;
    public TapObject prefabTextMousePosition;

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;

        currencyManager.SubscribeToEventSendCurrencyValue(UpdateCurrencyText);
        currencyManager.SubscribeToEventSendPassiveCurrencyGainValue(UpdatePassiveGainText);
        currencyManager.SubscribeToEventSendActiveCurrencyValue(InstantiateTapObject);
        //textDoubleGainTime.text = "";

        //SubscribeToEventShowOptionsPanel(FindObjectOfType<CanvasOptions>().MoveToPosition);
        //currencyManager.EventUpdateTextCurrency += UpdateCurrencyText;
        //currencyManager.EventUpdateTextIdleGain += UpdateIdleGainText;
        //currencyManager.EventUpdateTextDoubleGainTime += UpdateDoubleGainTime;
        //currencyManager.EventSendTouchPosition += InstantiateTapObject;
    }

    public void ShowOptionsPanel()
    {
        EventShowOptionsPanel?.Invoke();
    }

    public void SubscribeToEventShowOptionsPanel(Action method)
    {
        EventShowOptionsPanel += method;
    }

    public void UpdatePassiveGainText(double value)
    {
        textPassiveGain.text =  value != 0 ? $"+{Formatter.FormatValue(value)}/s" : "";
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
    private void InstantiateTapObject(double value, Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(value, currencyManager.spriteCurrency);
    }
}
