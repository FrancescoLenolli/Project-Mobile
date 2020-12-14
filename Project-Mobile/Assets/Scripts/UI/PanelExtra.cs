using UnityEngine;

public delegate void WatchAd(AdsManager.AdType adType);
public class PanelExtra : MonoBehaviour
{
    public event WatchAd EventWatchCurrencyAd;

    private GameManager gameManager = null;

    private void Start()
    {
        gameManager = GameManager.Instance;

        EventWatchCurrencyAd += gameManager.adsManager.ShowAd;
    }

    // Add a percentage of currency to the actual value.
    public void WatchAdCurrency()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.BaseCurrency);
    }

    public void WatchAdDoubleEarnings()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.DoubleIdleEarnings);
    }
}
