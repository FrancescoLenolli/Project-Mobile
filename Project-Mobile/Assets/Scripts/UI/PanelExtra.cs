using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void AddCurrency(long value);
public delegate void AddPremiumCurrency(int value);
public delegate void MultiplyIdleGain(float time);

public class PanelExtra : MonoBehaviour
{
    public event AddCurrency eventAddCurrency;
    public event AddPremiumCurrency eventAddPremiumCurrency;
    public event MultiplyIdleGain eventMultiplyIdleGain;

    private CurrencyManager currencyManager = null;

    [Tooltip("Percentage added to current currency/premium currency when tapping the respective buttons.\nEx: current currency is 100, if value is 20%, 20 currency will be added.")]
    [SerializeField] private float extraCurrencyPercentage = 0;

    [Tooltip("How much time will the IdleGain be multiplied for?")]
    [SerializeField] private float multiplierTime = 0;

    [SerializeField] private List<Button> listButtons = new List<Button>();

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;

        eventAddCurrency += currencyManager.AddMoreCurrency;
        eventAddPremiumCurrency += currencyManager.AddMorePremiumCurrency;
        eventMultiplyIdleGain += currencyManager.MultiplyIdleGain;
    }

    // Add a percentage of currency to the actual value.
    public void AddCurrency()
    {
        long value = Mathf.RoundToInt((currencyManager.currency * extraCurrencyPercentage) / 100);
        eventAddCurrency?.Invoke(value);

        StartCoroutine(ButtonCooldown(listButtons[0]));
    }

    // Add a percentage of premiumCurrency to the actual value.
    public void AddPremiumCurrency()
    {
        int value = Mathf.RoundToInt((currencyManager.premiumCurrency * extraCurrencyPercentage) / 100);
        eventAddPremiumCurrency?.Invoke(value);
        StartCoroutine(ButtonCooldown(listButtons[1]));
    }

    // Multiply the IdleGain for n time.
    // CURRENTLY HARD CODED TO DOUBLE IT.
    public void MultiplyIdleGain()
    {
        eventMultiplyIdleGain?.Invoke(multiplierTime);
        StartCoroutine(ButtonCooldown(listButtons[2], multiplierTime));
    }

    private IEnumerator ButtonCooldown(Button button, float time = 5.0f)
    {
        button.interactable = false;
        yield return new WaitForSeconds(time);
        button.interactable = true;
    }
}
