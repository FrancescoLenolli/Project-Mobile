using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void UpdateModifier(int newModifierValue);
public class CanvasBottom : MonoBehaviour
{
    public event UpdateModifier UpdateModifier;

    private CurrencyManager currencyManager = null;

    [HideInInspector] public int modifierValue = 0;

    public TextMeshProUGUI textModifier = null;
    public List<CanvasGroup> listPanels = new List<CanvasGroup>();

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
    }

    // Update Modifier for Ship Quantity to buy.
    // Player can buy one unit of Ship at a time, or more.
    // Modifiers are stored in currencyManager > listQuantityModifier.
    public void CycleModifiers()
    {
        modifierValue = currencyManager.CycleModifierAndReturnValue();
        textModifier.text = $"{modifierValue}";
        UpdateModifier?.Invoke(modifierValue);
    }

    // Open one panel and close the others.
    public void OpenPanel(int index)
    {
        for(int i = 0; i < listPanels.Count; ++i)
        {
            if(i == index)
            {
                ChangeStatus(listPanels[i], true);
            }
            else
            {

                ChangeStatus(listPanels[i], false);
            }
        }
    }

    // Set if a Canvas is visible or not based on his CanvasGroup.
    private void ChangeStatus(CanvasGroup canvasGroup, bool isVisible)
    {
        int alphaValue = isVisible ? 1 : 0;

        canvasGroup.alpha = alphaValue;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    Vector2 ResizeContainer(Transform container, Transform additionalElement, float additionalSpace)
    {
        RectTransform additionalElementRect = additionalElement.GetComponent<RectTransform>();
        RectTransform containerRect = container.GetComponent<RectTransform>();

        Vector2 containerUpdatedSize = new Vector2(containerRect.sizeDelta.x, containerRect.sizeDelta.y + additionalElementRect.sizeDelta.y + additionalSpace);
        return containerUpdatedSize;
    }
}
