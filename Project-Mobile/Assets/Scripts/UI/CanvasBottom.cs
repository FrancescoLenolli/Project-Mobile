using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void UpdateQuantityModifier(int newModifierValue);
public delegate void ShowOptionsPanel();
public class CanvasBottom : MonoBehaviour
{
    public enum BottomPanels { Ships, Upgrades, Extras }
    public event UpdateQuantityModifier EventUpdateQuantityModifier;
    public event ShowOptionsPanel EventShowOptionsPanel;

    private CurrencyManager currencyManager = null;
    private UIManager uiManager = null;

    [Tooltip("Which Panel will be visible when starting the game?")]
    [SerializeField] private BottomPanels firstActivePanel = BottomPanels.Ships;

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

        EventShowOptionsPanel += FindObjectOfType<CanvasOptions>().MoveToPosition;

        // At start of the game, make visible one panel while closing the others.
        OpenPanel((int)firstActivePanel);
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
        for (int i = 0; i < listPanels.Count; ++i)
        {
            if (i == index)
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
