using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void CollectBackgroundGain(CanvasBackgroundGain.CollectionType collectionType);

public class CanvasBackgroundGain : MonoBehaviour
{
    public enum CollectionType { Normal, DoubleAd, DoublePremium }

    public event CollectBackgroundGain eventCollectBackgroundGain;

    private UIManager uIManager = null;
    private Vector3 originalPosition = Vector3.zero;
    private CanvasGroup panelCanvasGroup = null;
    private int backgroundGain = 0;

    public Transform panelOfflineGain = null;
    public Transform newPosition = null;
    [Min(0)]
    public float animationTime = 0;
    public List<Button> listPanelButtons = new List<Button>();
    public TextMeshProUGUI textCurrencyGained = null;

    private void Start()
    {
        uIManager = UIManager.Instance;
        panelCanvasGroup = panelOfflineGain.GetComponent<CanvasGroup>();
        originalPosition = panelOfflineGain.localPosition;

        eventCollectBackgroundGain += CurrencyManager.Instance.AddBackgroundGain;
        CurrencyManager.Instance.eventBackgroundGainCalculated += ShowPanel;
    }

    private void HidePanel()
    {
        uIManager.MoveObjectAndFade(animationTime, panelOfflineGain, originalPosition, UIManager.Fade.Out);
    }

    public void ShowPanel(int currencyGained)
    {
        uIManager.MoveObjectAndFade(animationTime, panelOfflineGain, newPosition.localPosition, UIManager.Fade.In);
        backgroundGain = currencyGained;
        textCurrencyGained.text = backgroundGain.ToString();
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
        eventCollectBackgroundGain?.Invoke(collectionType);
        HidePanel();
        uIManager.ChangeAllButtons(listPanelButtons, false);
    }





}
