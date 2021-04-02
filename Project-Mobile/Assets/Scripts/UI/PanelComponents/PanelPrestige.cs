using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelPrestige : MonoBehaviour
{
    PrestigeManager prestigeManager;

    [SerializeField] private TextMeshProUGUI textWeight = null;
    [SerializeField] private Button buttonPrestige = null;

    public PanelAnimator panelAnimator;


    private void Start()
    {
        prestigeManager = PrestigeManager.Instance;
    }

    public void MoveToPosition()
    {
        bool isVisible = panelAnimator.IsPanelInView();
        panelAnimator.MoveToPosition();

        if(!isVisible)
        {
            SetData();
        }
    }

    public void SetData()
    {
        int currentWeight = prestigeManager.GetCollectiblesWeight();
        int requiredWeight = prestigeManager.requiredWeight;
        bool isWeightEnough = currentWeight >= requiredWeight;

        textWeight.text = $"{currentWeight}/{requiredWeight}";
        textWeight.color = isWeightEnough ? Color.green : Color.red;
        buttonPrestige.interactable = isWeightEnough;
    }
}
