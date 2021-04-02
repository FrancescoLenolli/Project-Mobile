using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelExtra : MonoBehaviour
{
    private Action<AdsManager.AdType> EventWatchCurrencyAd;

    private GameManager gameManager;
    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private CanvasBottom canvasBottom;
    private string title;
    private string description;
    private UnityAction adAction;
    private UnityAction buyAction;

    [SerializeField] private TextMeshProUGUI textTitle = null;
    [SerializeField] private TextMeshProUGUI textDescription = null;
    [SerializeField] private Button buttonAd = null;
    [SerializeField] private Button buttonBuy = null;

    public void InitData(CanvasBottom canvasBottom)
    {
        gameManager = GameManager.Instance;
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;
        this.canvasBottom = canvasBottom;

        Observer.AddObserver(ref EventWatchCurrencyAd, gameManager.adsManager.ShowAd);
    }

    public void SetUpPanel(AdsManager.AdType adType)
    {
        switch (adType)
        {
            case AdsManager.AdType.BaseCurrency:
                SetUpExtraGetCurrency();
                break;
            case AdsManager.AdType.DoubleIdleEarnings:
                SetUpExtraDoubleIdleEarnings();
                break;
            case AdsManager.AdType.PremiumCurrency:
                SetUpExtraGetCurrencyPremium();
                break;
            default:
                break;
        }
    }

    public void WatchAdExtraCurrency()
    {
        canvasBottom.MovePanelToPosition(true);
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.BaseCurrency);
    }

    public void BuyExtraCurrency()
    {
        if (currencyManager.BuyCurrencyFixedValue())
            canvasBottom.MovePanelToPosition(true);
    }

    public void WatchAdDoubleEarnings()
    {
        canvasBottom.MovePanelToPosition(true);
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.DoubleIdleEarnings);
    }

    public void BuyDoubleEarnings()
    {
        if (currencyManager.BuyDoubleGainTime())
            canvasBottom.MovePanelToPosition(true);
    }

    public void WatchAdGetPremiumCurrency()
    {
        canvasBottom.MovePanelToPosition(true);
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.PremiumCurrency);
    }

    private void SetUpExtraGetCurrency()
    {
        title = "Get Currency";
        description = $"Get {currencyManager.data.adPctGain}% of the actual Currency." +
            $"\n{Formatter.FormatValue(MathUtils.Pct(currencyManager.data.adPctGain, currencyManager.currency))}";
        adAction = WatchAdExtraCurrency;
        buyAction = BuyExtraCurrency;

        PanelSetup(title, description, buyAction, adAction);
    }

    private void SetUpExtraDoubleIdleEarnings()
    {
        title = "Double your IdleGain";
        description = $"Get {currencyManager.data.adHoursDoubleGain} hours of doubled idle gain";
        adAction = WatchAdDoubleEarnings;
        buyAction = BuyDoubleEarnings;

        PanelSetup(title, description, buyAction, adAction);
    }

    private void SetUpExtraGetCurrencyPremium()
    {
        title = "Get Premium Currency";
        description = $"Gain {currencyManager.data.adPremiumCurrencyGain} premium currency";
        adAction = WatchAdGetPremiumCurrency;

        PanelSetup(title, description, null, adAction, false);
    }

    private void PanelSetup(string title, string description, UnityAction buyAction, UnityAction adAction, bool isButtonBuyVisible = true)
    {
        uiManager.ChangeVisibility(buttonBuy.transform, isButtonBuyVisible);

        textTitle.text = title;
        textDescription.text = description;

        buttonAd.onClick.RemoveAllListeners();
        buttonBuy.onClick.RemoveAllListeners();

        if (adAction != null)
            buttonAd.onClick.AddListener(adAction);
        if (buyAction != null)
            buttonBuy.onClick.AddListener(buyAction);
    }
}
