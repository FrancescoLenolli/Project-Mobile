using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapObject : MonoBehaviour
{
    private CanvasGroup canvasGroup = null;
    public TextMeshProUGUI textCurrency = null;
    public Image spriteCurrency = null;
    [Min(0)]
    public float movementSpeed = 1;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // [!!!] Enumerator StartAnimation
        StartCoroutine(Animation());
    }

    public void SetValues(int currency /* Sprite currencySprite */)
    {
        textCurrency.text = $"+{currency}";
    }

    IEnumerator DelayDestroy(int delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    IEnumerator Animation()
    {
        while (canvasGroup.alpha > 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * (movementSpeed * 10), Space.Self);

            canvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }
}
