using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public delegate void UpdateQuantityModifier(int newModifierValue);
public delegate void ShowOptionsPanel();
public class CanvasBottom : MonoBehaviour
{
    public event UpdateQuantityModifier EventUpdateQuantityModifier;
    public event ShowOptionsPanel EventShowOptionsPanel;

    private CurrencyManager currencyManager = null;
    private UIManager uiManager = null;

    [HideInInspector] public int modifierValue = 0;
    [HideInInspector] public PanelShips panelShips = null;
    [HideInInspector] public PanelShipsUpgrades panelShipsUpgrades = null;

    public TextMeshProUGUI textModifier = null;
    public List<CanvasGroup> listPanels = new List<CanvasGroup>();

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;
        panelShips = GetComponentInChildren<PanelShips>();
        panelShipsUpgrades = GetComponentInChildren<PanelShipsUpgrades>();
    }

    // Update Modifier for Ship Quantity to buy.
    // Player can buy one unit of Ship at a time, or more.
    // Modifiers are stored in currencyManager > listQuantityModifier.
    public void CycleModifiers()
    {
        modifierValue = currencyManager.CycleModifierAndReturnValue();
        textModifier.text = $"{modifierValue}";
        EventUpdateQuantityModifier?.Invoke(modifierValue);
    }

    // Open one panel and close the others.
    public void OpenPanel(int index)
    {
        for(int i = 0; i < listPanels.Count; ++i)
        {
            if(i == index)
            {
                uiManager.ChangeStatus(listPanels[i], true);
            }
            else
            {
                uiManager.ChangeStatus(listPanels[i], false);
            }
        }
    }

    public void ShowOptionsPanel()
    {
        EventShowOptionsPanel?.Invoke();
    }
}
