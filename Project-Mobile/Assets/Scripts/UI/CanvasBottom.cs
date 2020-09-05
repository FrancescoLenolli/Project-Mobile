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
                listPanels[i].alpha = 1;
                listPanels[i].interactable = true;
            }
            else
            {

                listPanels[i].alpha = 0;
                listPanels[i].interactable = false;
            }
        }
    }
}
