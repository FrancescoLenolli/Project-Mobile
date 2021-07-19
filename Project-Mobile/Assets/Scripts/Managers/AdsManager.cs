using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public enum AdType { BaseCurrency, PremiumCurrency, DoubleIdleEarnings, DoubleOfflineEarnings }

    public Action EventAdBaseCurrency;
    public Action EventAdDoubleOfflineEarnings;
    public Action EventAdDoubleEarnings;
    public Action EventAdPremiumCurrency;

    private readonly string placement = "rewardedVideo";
    private AdType adType;

    [Tooltip("Time in HOURS where idle currency gain is doubled.")]
    public int doubleGainTime = 0;

    private void Start()
    {
        StartCoroutine(InitAd());

        CurrencyManager currencyManager = CurrencyManager.Instance;
        CanvasOfflineEarning canvasOfflineEarning = FindObjectOfType<CanvasOfflineEarning>();

        Observer.AddObserver(ref EventAdBaseCurrency, currencyManager.AddCurrencyFixedValue);
        Observer.AddObserver(ref EventAdPremiumCurrency, currencyManager.AddPremiumCurrencyFixedValue);
        Observer.AddObserver(ref EventAdDoubleEarnings, currencyManager.AddDoubleGainTime);
        Observer.AddObserver(ref EventAdDoubleOfflineEarnings, canvasOfflineEarning.CollectDoubleEarnings);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
            SelectAd(adType);
    }

    public void ShowAd(AdType adType)
    {
        bool showAds = true;

        SetCurrentAdType(adType);
        if (showAds)
            Advertisement.Show(placement);
        else
            SelectAd(this.adType);
    }

    private void SetCurrentAdType(AdType newType)
    {
        adType = newType;
    }

    private void SelectAd(AdType adType)
    {
        switch (adType)
        {
            case AdType.BaseCurrency:
                EventAdBaseCurrency?.Invoke();
                break;

            case AdType.DoubleIdleEarnings:
                EventAdDoubleEarnings?.Invoke();
                break;

            case AdType.DoubleOfflineEarnings:
                EventAdDoubleOfflineEarnings?.Invoke();
                break;
            case AdType.PremiumCurrency:
                EventAdPremiumCurrency?.Invoke();
                break;
            default:
                Debug.Log("Something went wrong with Ad rewards.");
                break;
        }
    }

    private IEnumerator InitAd()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("3884039", true);

        while (!Advertisement.IsReady(placement))
        {
            yield return null;
        }
    }

    #region UNUSED ADS METHODS
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }
    #endregion
}
