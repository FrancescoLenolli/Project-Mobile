using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void UpdateQuantityModifier(int newModifierValue);
public delegate void CycleLeft();
public delegate void CycleRight();
public class CanvasBottom : MonoBehaviour
{
    public enum BottomPanels { Ships, Upgrades, Extras }

    public event UpdateQuantityModifier EventUpdateQuantityModifier;
    public event CycleLeft EventCycleLeft;
    public event CycleRight EventCycleRight;

    private CurrencyManager currencyManager = null;
    private UIManager uiManager = null;

    [Tooltip("Which Panel will be visible when starting the game?")]
    [SerializeField] private BottomPanels firstActivePanel = BottomPanels.Ships;

    [HideInInspector] public int modifierValue = 0;
    [HideInInspector] public PanelShips panelShips = null;
    [HideInInspector] public PanelShipsUpgrades panelShipsUpgrades = null;

    public TextMeshProUGUI textQuantityModifier = null;
    public List<CanvasGroup> listPanels = new List<CanvasGroup>();

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;
        panelShips = GetComponentInChildren<PanelShips>();
        panelShipsUpgrades = GetComponentInChildren<PanelShipsUpgrades>();

        ShipsView shipsView = FindObjectOfType<ShipsView>();
        EventCycleLeft += shipsView.CycleLeft;
        EventCycleRight += shipsView.CycleRight;

        // At start of the game, make one panel visible while closing the others.
        OpenPanel((int)firstActivePanel);

        textQuantityModifier.text = $"{currencyManager.GetShipQuantityToBuy()}";
    }

    // Update Modifier for Ship Quantity to buy.
    // Player can buy one unit of Ship at a time, or more.
    // Modifiers are stored in currencyManager > listQuantityModifier.
    public void CycleModifiers()
    {
        modifierValue = currencyManager.CycleModifierAndReturnValue();
        textQuantityModifier.text = $"{modifierValue}";
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

    public void CycleLeft()
    {
        EventCycleLeft?.Invoke();
    }

    public void CycleRight()
    {
        EventCycleRight?.Invoke();
    }
}
