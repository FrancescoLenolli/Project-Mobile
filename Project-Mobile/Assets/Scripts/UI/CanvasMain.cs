using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void ShowOptionsPanel();
public class CanvasMain : MonoBehaviour
{
    private Action EventShowOptionsPanel;
    private Action EventPrestigeUp;

    private CurrencyManager currencyManager;
    private TextMeshProUGUI textPremiumCurrency;

    [SerializeField] private TextMeshProUGUI textCurrency = null;
    [SerializeField] private TextMeshProUGUI textPassiveGain = null;
    [SerializeField] private TextMeshProUGUI textDoubleGainTime = null;
    [SerializeField] private Button buttonPremiumCurrency = null;
    [SerializeField] private TapObject prefabTextMousePosition = null;

    public void InitData()
    {
        CanvasSettings canvasOptions = FindObjectOfType<CanvasSettings>();
        ShipsManager shipsManager = FindObjectOfType<ShipsManager>();
        GameManager gameManager = GameManager.Instance;

        currencyManager = CurrencyManager.Instance;
        textPremiumCurrency = buttonPremiumCurrency.GetComponentInChildren<TextMeshProUGUI>();

        buttonPremiumCurrency.image.sprite = currencyManager.data.premiumCurrencySprite;

        SubscribeToEventShowOptionsPanel(canvasOptions.MoveToPosition);
        SubscribeToEventPrestigeUp(gameManager.PrestigeUp);

        currencyManager.SubscribeToEventSendCurrency(UpdateCurrencyText);
        currencyManager.SubscribeToEventSendPremiumCurrency(UpdatePremiumCurrencyText);
        currencyManager.SubscribeToEventSendPassiveCurrencyGain(UpdatePassiveGainText);
        currencyManager.SubscribeToEventSendActiveCurrencyGain(InstantiateTapObject);
        currencyManager.SubscribeToEventSendDoubleGainTime(UpdateDoubleGainTime);
    }

    public void ShowOptionsPanel()
    {
        EventShowOptionsPanel?.Invoke();
    }

    public void PrestigeUp()
    {
        EventPrestigeUp?.Invoke();
    }

    public void UpdatePassiveGainText(double value)
    {
        textPassiveGain.text = $"+{Formatter.FormatValue(value)}/s";
    }

    public void UpdateCurrencyText(double value)
    {
        textCurrency.text = Formatter.FormatValue(value);
    }

    public void UpdatePremiumCurrencyText(int value)
    {
        textPremiumCurrency.text = value.ToString();
    }

    public void UpdateDoubleGainTime(double value)
    {
        textDoubleGainTime.text = value == 0 ? "" : $"x2 {TimeSpan.FromSeconds(value):hh\\:mm\\:ss}";
    }

    // When tapping on screen, instantiate object that displays the amount of currency gained by tapping.
    public void InstantiateTapObject(double value, Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(value, currencyManager.data.currencySprite);
    }


    public void SubscribeToEventShowOptionsPanel(Action method)
    {
        EventShowOptionsPanel += method;
    }

    public void SubscribeToEventPrestigeUp(Action method)
    {
        EventPrestigeUp += method;
    }
}
