﻿using System.Collections;
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
}