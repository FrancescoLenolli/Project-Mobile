using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void AddCurrency(long value);
public delegate void AddPremiumCurrency(int value);
public delegate void MultiplyIdleGain(float time);

public delegate void WatchAd(AdsManager.AdType adType);

public class PanelExtra : MonoBehaviour
{
    public event AddCurrency EventAddCurrency;
    public event AddPremiumCurrency EventAddPremiumCurrency;
    public event MultiplyIdleGain EventMultiplyIdleGain;

    public event WatchAd EventWatchCurrencyAd;

    private CurrencyManager currencyManager = null;

    [Tooltip("Percentage added to current currency/premium currency when tapping the respective buttons.\nEx: current currency is 100, if value is 20%, 20 currency will be added.")]
    [SerializeField] private float extraCurrencyPercentage = 0;

    [Tooltip("How much time will the IdleGain be multiplied for?")]
    [SerializeField] private float multiplierTime = 0;

    [SerializeField] private List<Button> listButtons = new List<Button>();

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;

        EventAddCurrency += currencyManager.AddMoreCurrency;
        EventAddPremiumCurrency += currencyManager.AddMorePremiumCurrency;
        EventMultiplyIdleGain += currencyManager.MultiplyIdleGain;

        EventWatchCurrencyAd += GameManager.Instance.adsManager.ShowAd;
    }

    // Add a percentage of currency to the actual value.
    public void WatchAdCurrency()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.BaseCurrency);
    }

    public void WatchAdPremium()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.PremiumCurrency);
    }

    public void WatchAdDoubleEarnings()
    {
        EventWatchCurrencyAd?.Invoke(AdsManager.AdType.DoubleEarnings);
    }

    // Add a percentage of premiumCurrency to the actual value.
    public void AddPremiumCurrency()
    {
        int value = Mathf.RoundToInt((currencyManager.premiumCurrency * extraCurrencyPercentage) / 100);
        EventAddPremiumCurrency?.Invoke(value);
        StartCoroutine(ButtonCooldown(listButtons[1]));
    }

    // Multiply the IdleGain for n time.
    // CURRENTLY HARD CODED TO DOUBLE IT.
    public void MultiplyIdleGain()
    {
        EventMultiplyIdleGain?.Invoke(multiplierTime);
        StartCoroutine(ButtonCooldown(listButtons[2], multiplierTime));
    }

    private IEnumerator ButtonCooldown(Button button, float time = 5.0f)
    {
        button.interactable = false;
        yield return new WaitForSeconds(time);
        button.interactable = true;
    }
}
