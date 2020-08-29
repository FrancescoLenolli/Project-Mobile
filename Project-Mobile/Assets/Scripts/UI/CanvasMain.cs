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
    public GameObject prefabTextMousePosition;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Start()
    {
        currencyManager.UpdateCurrencyText += UpdateCurrencyText;
        currencyManager.SpawnTextAtInputPosition += InstantiatePrefab;
    }

    private void UpdateCurrencyText(long value)
    {
        textCurrency.text = value.ToString(); 
    }

    private void InstantiatePrefab(Vector3 mousePosition)
    {
        GameObject prefab = Instantiate(prefabTextMousePosition, mousePosition, prefabTextMousePosition.transform.rotation, transform);
        prefab.GetComponent<TextMeshProUGUI>().text = mousePosition.ToString();
    }
}
