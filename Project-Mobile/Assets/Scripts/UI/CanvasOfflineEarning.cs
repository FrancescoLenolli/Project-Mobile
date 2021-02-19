using System;
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

    public Transform panelOfflineEarning = null;
    public Transform newPosition = null;
    [Min(0)]
    public float animationTime = 0;
    public List<Button> listPanelButtons = new List<Button>();
    public TextMeshProUGUI textCurrencyGained = null;
    public TextMeshProUGUI textOfflineTime = null;

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
        offlineEarnings = currencyGained;

        uiManager.MoveRectObjectAndFade(panelOfflineEarning, newPosition, animationTime, UIManager.Fade.In);

        textCurrencyGained.text = Formatter.FormatValue(currencyGained);
        textOfflineTime.text = string.Format("{0:hh\\:mm\\:ss}", timeOffline);
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

}
