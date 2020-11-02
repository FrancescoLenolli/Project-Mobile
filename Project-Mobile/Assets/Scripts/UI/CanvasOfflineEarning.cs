using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void CollectOfflineEarning(CanvasOfflineEarning.CollectionType collectionType);

public class CanvasOfflineEarning : MonoBehaviour
{
    public enum CollectionType { Normal, DoubleAd, DoublePremium }

    public event CollectOfflineEarning EventCollectOfflineEarning;

    private UIManager uIManager = null;
    private Vector3 originalPosition = Vector3.zero;
    private CanvasGroup panelCanvasGroup = null;
    private int offlineEarning = 0;

    public Transform panelOfflineEarning = null;
    public Transform newPosition = null;
    [Min(0)]
    public float animationTime = 0;
    public List<Button> listPanelButtons = new List<Button>();
    public TextMeshProUGUI textCurrencyGained = null;

    private void Start()
    {
        uIManager = UIManager.Instance;
        panelCanvasGroup = panelOfflineEarning.GetComponent<CanvasGroup>();
        originalPosition = panelOfflineEarning.localPosition;

        EventCollectOfflineEarning += CurrencyManager.Instance.AddBackgroundGain;
        CurrencyManager.Instance.EventBackgroundGainCalculated += ShowPanel;
    }

    private void HidePanel()
    {
        uIManager.MoveRectObjectAndFade(animationTime, panelOfflineEarning, originalPosition, UIManager.Fade.Out);
    }

    public void ShowPanel(int currencyGained)
    {
        uIManager.MoveRectObjectAndFade(animationTime, panelOfflineEarning, newPosition.localPosition, UIManager.Fade.In);
        offlineEarning = currencyGained;
        textCurrencyGained.text = offlineEarning.ToString();
    }

    public void DoubleWithPremium()
    {
        InvokeEventAndClose(CollectionType.DoublePremium);
    }

    public void DoubleWithAd()
    {
        InvokeEventAndClose(CollectionType.DoubleAd);
    }

    public void Collect()
    {
        InvokeEventAndClose(CollectionType.Normal);
    }

    private void InvokeEventAndClose(CollectionType collectionType)
    {
        EventCollectOfflineEarning?.Invoke(collectionType);
        HidePanel();
        uIManager.ChangeAllButtons(listPanelButtons, false);
    }





}
