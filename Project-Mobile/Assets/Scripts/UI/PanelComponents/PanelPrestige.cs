using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelPrestige : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDetails = null;
    [SerializeField] private Button buttonPrestige = null;

    public PanelAnimator panelAnimator;


    public void MoveToPosition()
    {
        bool isVisible = panelAnimator.IsPanelInView();
        panelAnimator.MoveToPosition();

        if(!isVisible)
        {
            InitPanel();
        }
    }

    private void InitPanel()
    {
        PrestigeManager prestigeManager = PrestigeManager.Instance;
        int currentWeight = prestigeManager.GetCollectiblesWeight();
        int requiredWeight = prestigeManager.requiredWeight;

        textDetails.text = $"Weight Required\n{currentWeight}/{requiredWeight}";
        buttonPrestige.interactable = currentWeight >= requiredWeight;
    }
}
