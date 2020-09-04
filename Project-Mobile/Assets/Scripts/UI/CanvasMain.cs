using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CanvasMain : MonoBehaviour
{
    CurrencyManager currencyManager;

    public TextMeshProUGUI textCurrency;
    public TapObject prefabTextMousePosition;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Start()
    {
        textCurrency.text = CurrencyManager.Instance.currency.ToString();

        currencyManager.UpdateCurrencyText += UpdateCurrencyText;
        currencyManager.SpawnTextAtInputPosition += InstantiatePrefab;
    }

    private void UpdateCurrencyText(long value)
    {
        textCurrency.text = value.ToString(); 
    }

    private void InstantiatePrefab(Vector3 mousePosition)
    {
        TapObject newTapObject = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        newTapObject.SetValues(currencyManager.currencyActiveGain * currencyManager.modifierActiveGain);
    }
}
