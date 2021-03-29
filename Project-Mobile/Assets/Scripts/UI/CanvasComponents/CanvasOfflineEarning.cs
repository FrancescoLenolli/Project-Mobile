using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOfflineEarning : MonoBehaviour
{
    public enum CollectionType { Normal, DoubleAd }

    private GameManager gameManager;
    private CurrencyManager currencyManager;
    private double offlineEarnings;

    [SerializeField] private TextMeshProUGUI textCurrencyGained = null;
    [SerializeField] private TextMeshProUGUI textOfflineTime = null;
    [SerializeField] private PanelAnimator panelAnimator = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        currencyManager = CurrencyManager.Instance;
    }
    
    public void CollectEarnings()
    {
        currencyManager.AddCurrency(offlineEarnings);
        HidePanel();
    }

    public void WatchAdForDoubleEarnings()
    {
        gameManager.adsManager.ShowAd(AdsManager.AdType.DoubleOfflineEarnings);
    }

    public void ShowPanel(TimeSpan timeOffline, double currencyGained)
    {
        StartCoroutine(WaitToShowPanel(3, timeOffline, currencyGained));
    }

    public void CollectDoubleEarnings()
    {
        currencyManager.AddCurrency(offlineEarnings * 2);
        HidePanel();
    }

    private void HidePanel()
    {
        panelAnimator.HidePanel();
    }

    private IEnumerator WaitToShowPanel(float time, TimeSpan timeOffline, double currencyGained)
    {
        yield return new WaitForSeconds(time);

        offlineEarnings = currencyGained;

        panelAnimator.ShowPanel();

        textCurrencyGained.text = Formatter.FormatValue(currencyGained);
        textOfflineTime.text = string.Format("{0:hh\\:mm\\:ss}", timeOffline);

        yield return null;
    }

}
