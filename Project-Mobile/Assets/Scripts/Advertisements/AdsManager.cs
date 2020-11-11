using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public delegate void AdBaseCurrency();
public delegate void AdDoubleEarnings();
public delegate void AdDoubleOfflineEarnings();
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public event AdBaseCurrency EventAdBaseCurrency;
    public event AdDoubleEarnings EventAdDoubleEarnings;
    public event AdDoubleOfflineEarnings EventAdDoubleOfflineEarnings;

    public enum AdType { BaseCurrency, DoubleIdleEarnings, DoubleOfflineEarnings }

    private string placement = "rewardedVideo";
    private AdType watchedAdType;

    private void Start()
    {
        StartCoroutine(InitAd());

        EventAdBaseCurrency += CurrencyManager.Instance.AddCurrencyAdvertisement;
        EventAdDoubleEarnings += CurrencyManager.Instance.AddDoubleIdleGainTime;
        EventAdDoubleOfflineEarnings += FindObjectOfType<CanvasOfflineEarning>().CollectDouble;
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
        SetAdWatchedType(adType);
        Advertisement.Show(placement);
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
}
