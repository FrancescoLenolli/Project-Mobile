using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public delegate void AdBaseCurrency();
public delegate void AdDoubleEarnings();
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public event AdBaseCurrency EventAdBaseCurrency;
    public event AdDoubleEarnings EventAdDoubleEarnings;

    public enum AdType { BaseCurrency, DoubleEarnings }

    private string placement = "rewardedVideo";
    private AdType watchedAdType;

    private void Start()
    {
        Advertisement.AddListener(this);
        EventAdBaseCurrency += CurrencyManager.Instance.AddCurrencyAdvertisement;
        EventAdDoubleEarnings += CurrencyManager.Instance.AddDoubleIdleGainTime;
    }

    private void SetAdWatchedType(AdType newType)
    {
        watchedAdType = newType;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            switch (watchedAdType)
            {
                case AdType.BaseCurrency:
                    EventAdBaseCurrency?.Invoke();
                    break;

                case AdType.DoubleEarnings:
                    EventAdDoubleEarnings?.Invoke();
                    break;

                default:
                    Debug.Log("Something went wrong with Ad rewards.");
                    break;
            }
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    // Subscribe this to some UI Event
    public void ShowAd(AdType adType)
    {
        StartCoroutine(StartAd(adType));
    }

    private IEnumerator StartAd(AdType adType)
    {
        Advertisement.Initialize("3884039", true);

        while (!Advertisement.IsReady(placement))
        {
            yield return null;
        }

        SetAdWatchedType(adType);

        Advertisement.Show(placement);
    }
}
