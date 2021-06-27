using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapObject : MonoBehaviour
{
    public TextMeshProUGUI textCurrency = null;
    public Image imageCurrency = null;
    [Min(0)]
    public float movementSpeed = 1;

    private CanvasGroup canvasGroup = null;

    public void Init(double value, Sprite currencySprite)
    {
        canvasGroup = GetComponent<CanvasGroup>();

        textCurrency.text = "+" + Formatter.FormatValue(value);
        imageCurrency.sprite = currencySprite;

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        while (canvasGroup.alpha > 0)
        {
            transform.Translate((movementSpeed * 10) * Time.deltaTime * Vector3.up, Space.Self);

            canvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }
}
