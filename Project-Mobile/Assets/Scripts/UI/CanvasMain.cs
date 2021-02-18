using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void ShowOptionsPanel();
public class CanvasMain : MonoBehaviour
{
    private Action EventShowOptionsPanel;
    private Action<UIManager.Cycle> EventCycleShipsModel;

    private CurrencyManager currencyManager;
    private TextMeshProUGUI textPremiumCurrency;

    public TextMeshProUGUI textCurrency;
    public TextMeshProUGUI textPassiveGain;
    public TextMeshProUGUI textDoubleGainTime;
    public Button buttonPremiumCurrency;
    public TapObject prefabTextMousePosition;

    public void InitData()
    {
        CanvasSettings canvasOptions = FindObjectOfType<CanvasSettings>();
        ShipsManager shipsManager = FindObjectOfType<ShipsManager>();

        currencyManager = CurrencyManager.Instance;
        textPremiumCurrency = buttonPremiumCurrency.GetComponentInChildren<TextMeshProUGUI>();

        buttonPremiumCurrency.image.sprite = currencyManager.spritePremiumCurrency;

        SubscribeToEventShowOptionsPanel(canvasOptions.MoveToPosition);
        SubscribeToEventCycleShipsModel(shipsManager.CycleModels);
        currencyManager.SubscribeToEventSendCurrency(UpdateCurrencyText);
        currencyManager.SubscribeToEventSendPremiumCurrency(UpdatePremiumCurrencyText);
        currencyManager.SubscribeToEventSendPassiveCurrencyGain(UpdatePassiveGainText);
        currencyManager.SubscribeToEventSendActiveCurrencyGain(InstantiateTapObject);
    }

    public void ShowOptionsPanel()
    {
        EventShowOptionsPanel?.Invoke();
    }

    public void CycleModelsLeft()
    {
        EventCycleShipsModel?.Invoke(UIManager.Cycle.Left);
    }

    public void CycleModelsRight()
    {
        EventCycleShipsModel?.Invoke(UIManager.Cycle.Right);
    }

    public void UpdatePassiveGainText(double value)
    {
        textPassiveGain.text =  value != 0 ? $"+{Formatter.FormatValue(value)}/s" : "";
    }

    public void UpdateCurrencyText(double value)
    {
        textCurrency.text = Formatter.FormatValue(value);
    }

    public void UpdatePremiumCurrencyText(int value)
    {
        textPremiumCurrency.text = Formatter.FormatValue(value);
    }

    public void UpdateDoubleGainTime(int value)
    {
        textDoubleGainTime.text = value == 0 ? "" : $"x2 {TimeSpan.FromSeconds(value)}";
    }

    // When tapping on screen, instantiate object that displays the amount of currency gained by tapping.
    public void InstantiateTapObject(double value, Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(value, currencyManager.spriteCurrency);
    }


    public void SubscribeToEventShowOptionsPanel(Action method)
    {
        EventShowOptionsPanel += method;
    }

    public void SubscribeToEventCycleShipsModel(Action<UIManager.Cycle> method)
    {
        EventCycleShipsModel += method;
    }
}
