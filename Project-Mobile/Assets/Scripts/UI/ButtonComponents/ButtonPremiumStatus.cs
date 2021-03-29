using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPremiumStatus : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private Button button;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
        button = GetComponent<Button>();
        StartCoroutine(SetButtonStatus());
    }

    private IEnumerator SetButtonStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(.3f);
            button.interactable = currencyManager.CanBuyWithPremium();
            yield return null;
        }
    }
}
