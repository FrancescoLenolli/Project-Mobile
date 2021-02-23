using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public delegate void AdBaseCurrency();
public delegate void AdDoubleEarnings(int doubleGainTime);
public delegate void AdDoubleOfflineEarnings();
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public enum AdType { BaseCurrency, DoubleIdleEarnings, DoubleOfflineEarnings }

    private Action EventAdBaseCurrency;
    private Action EventAdDoubleOfflineEarnings;
    private Action EventAdDoubleEarnings;

    private string placement = "rewardedVideo";
    private AdType adType;

    [Tooltip("Time in HOURS where idle currency gain is doubled.")]
    public int doubleGainTime = 0;

    private void Start()
    {
        StartCoroutine(InitAd());

        SubscribeToEventAdBaseCurrency(CurrencyManager.Instance.AddCurrencyFixedValue);
        SubscribeToEventAdDoubleEarnings(CurrencyManager.Instance.AddDoubleGainTime);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
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
                default:
                    Debug.Log("Something went wrong with Ad rewards.");
                    break;
            }
        }
    }

    public void ShowAd(AdType adType)
    {
        SetCurrentAdType(adType);
        Advertisement.Show(placement);
    }


    public void SubscribeToEventAdBaseCurrency(Action method)
    {
        EventAdBaseCurrency += method;
    }
    public void SubscribeToEventAdDoubleOfflineEarnings(Action method)
    {
        EventAdDoubleOfflineEarnings += method;
    }
    public void SubscribeToEventAdDoubleEarnings(Action method)
    {
        EventAdDoubleEarnings += method;
    }


    private void SetCurrentAdType(AdType newType)
    {
        adType = newType;
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
