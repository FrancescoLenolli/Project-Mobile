using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void WatchAd(AdsManager.AdType adType);
public class PanelExtra : MonoBehaviour
{
    public event WatchAd EventWatchCurrencyAd;

    private CurrencyManager currencyManager = null;
    private UIManager uiManager = null;

    [SerializeField] private List<Button> listButtons = new List<Button>();

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        EventWatchCurrencyAd += GameManager.Instance.adsManager.ShowAd;
    }

    // Add a percentage of currency to the actual value.
    public void WatchAdCurrency()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.BaseCurrency);
    }

    public void WatchAdDoubleEarnings()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.DoubleEarnings);
    }
}
