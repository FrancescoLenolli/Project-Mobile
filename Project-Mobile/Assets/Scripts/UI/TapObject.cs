using System.Collections;
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
        StartCoroutine(Animation());
    }

    // Print how much currency is gained by tapping on screen.
    public void SetValues(int currency /* Sprite currencySprite */)
    {
        textCurrency.text = "+" + Formatter.FormatValue(currency);
    }

    // After instantiating the object, start a little animation.
    // The object moves, fades out and then get destroyed.
    private IEnumerator Animation()
    {
        // While the object is visible, move up and progressively fade away.
        while (canvasGroup.alpha > 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * (movementSpeed * 10), Space.Self);

            canvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }

        // Destroy as soon as the object becomes invisible.
        Destroy(gameObject);
        yield return null;
    }
}
