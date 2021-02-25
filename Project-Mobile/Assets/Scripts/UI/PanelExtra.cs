using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelExtra : MonoBehaviour
{
    private Action<AdsManager.AdType> EventWatchCurrencyAd;

    private GameManager gameManager;
    private CurrencyManager currencyManager;
    private CanvasBottom canvasBottom;

    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textDescription;
    public Button buttonAd;
    public Button buttonBuy;

    public void InitData(CanvasBottom canvasBottom)
    {
        gameManager = GameManager.Instance;
        currencyManager = CurrencyManager.Instance;
        this.canvasBottom = canvasBottom;

        SubscribeToEventWatchAd(gameManager.adsManager.ShowAd);
    }

    public void SetUpPanel(AdsManager.AdType adType)
    {
        switch (adType)
        {
            case AdsManager.AdType.BaseCurrency:
                textTitle.text = "Get Currency";
                textDescription.text = $"Get {currencyManager.data.adPctGain}% of the actual Currency.\n{MathUtils.Pct(currencyManager.data.adPctGain, currencyManager.currency)}";
                buttonAd.onClick.RemoveAllListeners();
                buttonBuy.onClick.RemoveAllListeners();
                buttonAd.onClick.AddListener(WatchAdExtraCurrency);
                buttonBuy.onClick.AddListener(BuyExtraCurrency);
                break;
            case AdsManager.AdType.DoubleIdleEarnings:
                textTitle.text = "Double your IdleGain";
                textDescription.text = $"Get {currencyManager.data.adHoursDoubleGain} hours of doubled idle gain";
                buttonAd.onClick.RemoveAllListeners();
                buttonBuy.onClick.RemoveAllListeners();
                buttonAd.onClick.AddListener(WatchAdDoubleEarnings);
                buttonBuy.onClick.AddListener(BuyDoubleEarnings);
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


    private void SubscribeToEventWatchAd(Action<AdsManager.AdType> method)
    {
        EventWatchCurrencyAd += method;
    }
}
