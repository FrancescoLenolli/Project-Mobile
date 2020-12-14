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

    private CurrencyManager currencyManager = null;
    private string placement = "rewardedVideo";
    private AdType adType;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Start()
    {
        StartCoroutine(InitAd());

        EventAdBaseCurrency += currencyManager.AddCurrencyAdvertisement;
        EventAdDoubleEarnings += currencyManager.AddDoubleIdleGainTime;
        EventAdDoubleOfflineEarnings += FindObjectOfType<CanvasOfflineEarning>().CollectDouble;
    }

    // Set which type of Ad will be watched next.
    private void SetCurrentAdType(AdType newType)
    {
        adType = newType;
    }

    // When the Ad has been completely watched, give the right reward to the Player.
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            // Different behaviour depending on the Ad watched.
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

    // Normally it's not the best idea to use a Region.
    // Here I'm simply hiding some unused standard methods.
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

    // Set the type of the Ad to watch, then show it.
    public void ShowAd(AdType adType)
    {
        SetCurrentAdType(adType);
        Advertisement.Show(placement);
    }

    // Prepare the Advertisement.
    // Avoid delays when the Player wants to watch an Ad.
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
