using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapObject : MonoBehaviour
{
    public TextMeshProUGUI textCurrency = null;
    public Image spriteCurrency = null;

    void Start()
    {
        // [!!!] Enumerator StartAnimation
        StartCoroutine(DelayDestroy(2));
    }

    public void SetValues(int currency /* Sprite currencySprite */)
    {
        textCurrency.text = $"+{currency}";
    }

    IEnumerator DelayDestroy(int newDelay)
    {
        yield return new WaitForSeconds(newDelay);
        Destroy(gameObject);
    }
}
