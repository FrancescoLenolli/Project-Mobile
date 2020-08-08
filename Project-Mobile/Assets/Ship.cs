using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    // [!!!] ShipManager passes a list of shipData to the Canvas, the Canvas assign every shipData with isAvailable TRUE, instantiate the Ship UI Prefab, that Initialise his values
    [HideInInspector] public ShipData shipData = null;
    public Image imageIcon = null;
    public TextMeshProUGUI textQuantity = null;
    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textCurrencyGain = null;
    public TextMeshProUGUI textAdditionalCurrencyGain = null;
    public TextMeshProUGUI textCost = null;
    public TextMeshProUGUI textQuantityMultiplier = null;


    public void SetValues()
    {
        imageIcon.sprite = shipData.shipIcon;
        textQuantity.text = $"{shipData.quantity}";
        textName.text = shipData.name;
        textCurrencyGain.text = $"{shipData.currencyGain * shipData.quantity}/s";
        textAdditionalCurrencyGain.text = $"{shipData.currencyGain}/s"; // * MULTIPLIER
        textCost.text = $"{shipData.cost}"; // * MULTIPLIER
    }


}
