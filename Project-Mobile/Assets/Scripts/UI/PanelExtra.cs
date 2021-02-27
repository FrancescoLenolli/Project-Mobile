using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelExtra : MonoBehaviour
{
    private Action<AdsManager.AdType> EventWatchCurrencyAd;

    private GameManager gameManager;
    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private CanvasBottom canvasBottom;

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

        SubscribeToEventWatchAd(gameManager.adsManager.ShowAd);
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


    private void SubscribeToEventWatchAd(Action<AdsManager.AdType> method)
    {
        EventWatchCurrencyAd += method;
    }


    private void SetUpExtraGetCurrency()
    {
        uiManager.ChangeVisibility(buttonBuy.transform, true);

        textTitle.text = "Get Currency";
        textDescription.text = $"Get {currencyManager.data.adPctGain}% of the actual Currency." +
            $"\n{MathUtils.Pct(currencyManager.data.adPctGain, currencyManager.currency)}";
        buttonAd.onClick.RemoveAllListeners();
        buttonBuy.onClick.RemoveAllListeners();
        buttonAd.onClick.AddListener(WatchAdExtraCurrency);
        buttonBuy.onClick.AddListener(BuyExtraCurrency);
    }

    private void SetUpExtraDoubleIdleEarnings()
    {
        uiManager.ChangeVisibility(buttonBuy.transform, true);

        textTitle.text = "Double your IdleGain";
        textDescription.text = $"Get {currencyManager.data.adHoursDoubleGain} hours of doubled idle gain";
        buttonAd.onClick.RemoveAllListeners();
        buttonBuy.onClick.RemoveAllListeners();
        buttonAd.onClick.AddListener(WatchAdDoubleEarnings);
        buttonBuy.onClick.AddListener(BuyDoubleEarnings);
    }

    private void SetUpExtraGetCurrencyPremium()
    {
        uiManager.ChangeVisibility(buttonBuy.transform, false);

        textTitle.text = "Get Premium Currency";
        textDescription.text = $"Gain {currencyManager.data.adPremiumCurrencyGain} premium currency";
        buttonAd.onClick.RemoveAllListeners();
        buttonBuy.onClick.RemoveAllListeners();
        buttonAd.onClick.AddListener(WatchAdGetPremiumCurrency);
    }
}
