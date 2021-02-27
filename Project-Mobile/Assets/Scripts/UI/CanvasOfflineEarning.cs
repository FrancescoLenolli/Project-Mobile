using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOfflineEarning : MonoBehaviour
{
    public enum CollectionType { Normal, DoubleAd }

    private UIManager uiManager;
    private GameManager gameManager;
    private CurrencyManager currencyManager;
    private Vector3 originalPosition;
    private double offlineEarnings;

    [SerializeField] private Transform panelOfflineEarning = null;
    [SerializeField] private Transform newPosition = null;
    [SerializeField] private float animationTime = 0;
    [SerializeField] private TextMeshProUGUI textCurrencyGained = null;
    [SerializeField] private TextMeshProUGUI textOfflineTime = null;

    private void Start()
    {
        uiManager = UIManager.Instance;
        gameManager = GameManager.Instance;
        currencyManager = CurrencyManager.Instance;

        gameManager.adsManager.SubscribeToEventAdDoubleOfflineEarnings(CollectDoubleEarnings);
        originalPosition = panelOfflineEarning.localPosition;
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

    private void CollectDoubleEarnings()
    {
        currencyManager.AddCurrency(offlineEarnings * 2);
        HidePanel();
    }

    private void HidePanel()
    {
        uiManager.MoveRectObjectAndFade(panelOfflineEarning, originalPosition, animationTime, UIManager.Fade.Out);
    }

    private IEnumerator WaitToShowPanel(float time, TimeSpan timeOffline, double currencyGained)
    {
        yield return new WaitForSeconds(time);

        offlineEarnings = currencyGained;

        uiManager.MoveRectObjectAndFade(panelOfflineEarning, newPosition, animationTime, UIManager.Fade.In);

        textCurrencyGained.text = Formatter.FormatValue(currencyGained);
        textOfflineTime.text = string.Format("{0:hh\\:mm\\:ss}", timeOffline);

        yield return null;
    }

}
