using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void CollectOfflineEarning(CanvasOfflineEarning.CollectionType collectionType);

public class CanvasOfflineEarning : MonoBehaviour
{
    public enum CollectionType { Normal, DoubleAd }

    public event CollectOfflineEarning EventCollectOfflineEarning;
    public event WatchAd EventWatchAd;

    private UIManager uIManager = null;
    private GameManager gameManager = null;
    private CurrencyManager currencyManager = null;

    private Vector3 originalPosition = Vector3.zero;
    private int offlineEarning = 0;

    public Transform panelOfflineEarning = null;
    public Transform newPosition = null;
    [Min(0)]
    public float animationTime = 0;
    public List<Button> listPanelButtons = new List<Button>();
    public TextMeshProUGUI textCurrencyGained = null;
    public TextMeshProUGUI textOfflineTime = null;

    private void Start()
    {
        uIManager = UIManager.Instance;
        gameManager = GameManager.Instance;
        currencyManager = CurrencyManager.Instance;

        originalPosition = panelOfflineEarning.localPosition;

        EventCollectOfflineEarning += currencyManager.AddOfflineEarnings;
        EventWatchAd += gameManager.adsManager.ShowAd;
        currencyManager.EventBackgroundGainCalculated += ShowPanel;
    }

    private void HidePanel()
    {
        uIManager.MoveRectObjectAndFade(animationTime, panelOfflineEarning, originalPosition, UIManager.Fade.Out);
    }

    // Display panel on screen.
    // Don't do it if it's the first time the Player plays the game.
    public void ShowPanel(int currencyGained)
    {
            uIManager.MoveRectObjectAndFade(animationTime, panelOfflineEarning, newPosition.localPosition, UIManager.Fade.In);
            offlineEarning = currencyGained;
            textCurrencyGained.text = offlineEarning.ToString();
            textOfflineTime.text = string.Format("While you were on vacation for {0:hh\\:mm\\:ss}, LunaSolution gained:", GameManager.Instance.timeOffline);
    }

    // Called when the Player chooses to double the offline earnings by watching an Ad.
    public void DoubleGain()
    {
        EventWatchAd?.Invoke(AdsManager.AdType.DoubleOfflineEarnings);
    }

    // Called by AdsManager after watching ad, doubles offline earnings.
    public void CollectDouble()
    {
        InvokeEventAndClose(CollectionType.DoubleAd);
    }

    // Collect offline earnings without doubling them.
    public void Collect()
    {
        InvokeEventAndClose(CollectionType.Normal);
    }

    // Collect Offline Earnings and close the panel.
    private void InvokeEventAndClose(CollectionType collectionType)
    {
        EventCollectOfflineEarning?.Invoke(collectionType);

        uIManager.ChangeAllButtons(listPanelButtons, false);
        HidePanel();
    }





}
