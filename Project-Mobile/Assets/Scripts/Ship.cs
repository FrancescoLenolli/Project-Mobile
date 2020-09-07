using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void BoughtShip(string shipName); // [!!!] Use event to unlock upgrades.
public class Ship : MonoBehaviour
{
    public event BoughtShip BoughtShip;

    private CurrencyManager currencyManager = null;
    private ShipsManager shipsManager = null;
    private CanvasBottom canvasBottom = null;

    public int quantity = 0;
    private int cost = 0;
    private int currencyGain = 0;
    // CurrencyGain increased if more X units are bought.
    private int additionalCurrencyGain = 0;
    private int quantityMultiplier = 0;

    [HideInInspector] public ShipData shipData = null;
    public Image imageIcon = null;
    public TextMeshProUGUI textQuantity = null;
    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textCurrencyGain = null;
    public TextMeshProUGUI textAdditionalCurrencyGain = null;
    public TextMeshProUGUI textCost = null;
    public TextMeshProUGUI textQuantityMultiplier = null;
    public Button buttonBuy = null;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
        shipsManager = ShipsManager.Instance;
    }

    private void Start()
    {
        canvasBottom = FindObjectOfType<CanvasBottom>();

        canvasBottom.UpdateModifier += UpdateModifier;
    }

    private void Update()
    {

        buttonBuy.interactable = currencyManager.currency >= cost ? true : false; // [!!!] A Sprite change is better, keep that in mind.

    }

    public void SetValues(ShipData newShipData, int newQuantity)
    {
        shipData = newShipData;
        quantity = newQuantity;

        quantityMultiplier = currencyManager.ReturnModifierValue();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        textName.text = shipData.shipName;
        imageIcon.sprite = shipData.shipIcon;
        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";

        currencyManager.ChangeCurrencyIdleGain(currencyGain * quantity);
    }

    public void UpdateValues()
    {
        quantityMultiplier = currencyManager.ReturnModifierValue();
        cost = shipData.cost * quantityMultiplier;
        currencyGain = shipData.currencyGain;
        additionalCurrencyGain = shipData.currencyGain * quantityMultiplier;

        textCost.text = $"{cost}";
        textQuantity.text = $"{quantity}";
        textQuantityMultiplier.text = $"{quantityMultiplier}";
        textCurrencyGain.text = $"{currencyGain * quantity}/s";
        textAdditionalCurrencyGain.text = $"{additionalCurrencyGain}/s";
    }

    // Method called from UI button to buy units of a Ship.
    public void Buy()
    {
        BoughtShip?.Invoke(shipData.shipName);

        int multiplier = quantityMultiplier;

        if (currencyManager.currency >= cost * multiplier)
        {
            currencyManager.currency -= cost;
            quantity += multiplier;
            currencyManager.ChangeCurrencyIdleGain(currencyGain * multiplier);
            UpdateValues();
            UpdateQuantity(shipData.index, quantity);

            if (quantity >= shipData.qtToUnlockNextShip) UnlockNextShip(shipData.index);

            // Play sound.
        }
        else
        {
            // Play different sound.
        }
    }

    private void UpdateModifier(int newModifierValue)
    {
        quantityMultiplier = newModifierValue;
        UpdateValues();
    }

    // Unlock next Ship once the right quantity has been reached.
    private void UnlockNextShip(int index)
    {
        shipsManager.AddNewShip(index);
    }

    // Update Quantity of this Ship in ShipInfo, that will be saved.
    private void UpdateQuantity(int index, int quantity)
    {
        shipsManager.UpdateQuantityAt(index, quantity);
    }

}
