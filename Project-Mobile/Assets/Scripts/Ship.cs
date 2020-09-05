using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void BoughtShip(string shipName); // [!!!] Use event to unlock upgrades.
public class Ship : MonoBehaviour
{
    public event BoughtShip BoughtShip;

    private CurrencyManager currencyManager = null;
    private CanvasBottom canvasBottom = null;

    public int quantity = 0;
    private int cost = 0;
    private int currencyGain = 0;
    // CurrencyGain increased if more X units are bought.
    private int additionalCurrencyGain = 0;
    private int quantityMultiplier = 0;

    // [!!!] ShipManager passes a list of shipData to the Canvas, the Canvas assign every shipData with isAvailable TRUE, instantiate the Ship UI Prefab, that Initialise his values
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

        if (currencyManager.currency >= cost * quantityMultiplier)
        {
            currencyManager.currency -= cost;
            quantity += quantityMultiplier;
            currencyManager.ChangeCurrencyIdleGain(currencyGain * quantityMultiplier);
            UpdateValues();

            if (quantity >= shipData.qtToUnlockNextShip) UnlockNextShip();

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

    private void UnlockNextShip()
    {
        ShipsManager.Instance.AddNewShip(shipData.index);
    }

}
