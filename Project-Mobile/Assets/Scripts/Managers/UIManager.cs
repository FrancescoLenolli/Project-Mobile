using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
    public enum Fade { In, Out }
    public enum Scale { Up, Down }

    private bool canAnimate = true;

    [Tooltip("Check if you want to change the current font with the one chosen below on Start.")]
    [SerializeField] private bool changeGlobalFontOnPlay = false;
    [SerializeField] private TMP_FontAsset font = null;


    private new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if(changeGlobalFontOnPlay)
        {
            ChangeGlobalFont();
        }
    }

    /// <summary>
    /// Move Canvas Element to newPosition in n seconds.
    /// </summary>
    public void MoveRectObject(float time, Transform animatedObject, Transform newPosition)
    {
        if (canAnimate)
            StartCoroutine(MoveRect(time, animatedObject, newPosition));
    }

    /// <summary>
    /// Move Canvas Element to newPosition in n seconds.
    /// </summary>
    public void MoveRectObject(float time, Transform animatedObject, Vector3 newPosition)
    {
        if (canAnimate)
            StartCoroutine(MoveRect(time, animatedObject, newPosition));
    }

    /// <summary>
    /// Move Canvas Element to newPosition in n seconds, with a Fade effect.
    /// </summary>
    public void MoveRectObjectAndFade(Transform animatedObject, Transform newPosition, float time, Fade fadeType)
    {
        if (canAnimate)
            StartCoroutine(MoveRectAndFade(time, animatedObject, newPosition, fadeType));
    }

    /// <summary>
    /// Move Canvas Element to newPosition in n seconds, with a Fade effect.
    /// </summary>
    public void MoveRectObjectAndFade(Transform animatedObject, Vector3 newPosition, float time, Fade fadeType)
    {
        if (canAnimate)
            StartCoroutine(MoveRectAndFade(time, animatedObject, newPosition, fadeType));
    }

    /// <summary>
    /// Basic animation, fade the element to make it visible/invisible.
    /// </summary>
    public void TimedFadeUIElement(CanvasGroup element, Fade fadeType, float fadeTime)
    {
        StartCoroutine(FadeUIElement(element, fadeType, fadeTime));
    }

    /// <summary>
    /// Basic animation, change the size of the element in a given amount of time.
    /// </summary>
    public void TimedScaleUIElement(Transform element, Scale scaleType, float scaleTime)
    {
        StartCoroutine(ScaleUIElement(element, scaleType, scaleTime));
    }

    private void ChangeGlobalFont()
    {
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            text.font = font;
        }
    }

    private IEnumerator MoveRect(float time, Transform animatedObject, Transform newPosition)
    {
        canAnimate = false;

        float duration = 0;
        Vector3 originalPosition = animatedObject.localPosition;
        Vector3 endPosition = newPosition.localPosition;

        while (duration < time)
        {
            float t2 = Time.deltaTime + duration / time > 1 ? 1 : Time.deltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, endPosition, t2);
            duration += Time.deltaTime;
            yield return null;
        }
        animatedObject.localPosition = endPosition;

        canAnimate = true;

        yield return null;
    }

    private IEnumerator MoveRect(float time, Transform animatedObject, Vector3 newPosition)
    {
        canAnimate = false;

        float duration = 0;
        Vector3 originalPosition = animatedObject.localPosition;

        while (duration < time)
        {
            float t2 = Time.deltaTime + duration / time > 1 ? 1 : Time.deltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, newPosition, t2);
            duration += Time.deltaTime;
            yield return null;
        }
        animatedObject.localPosition = newPosition;

        canAnimate = true;

        yield return null;
    }

    private IEnumerator MoveRectAndFade(float time, Transform animatedObject, Transform newPosition, Fade fadeType)
    {
        canAnimate = false;

        float duration = 0;
        Vector3 originalPosition = animatedObject.localPosition;
        Vector3 endPosition = newPosition.localPosition;
        CanvasGroup canvasGroup = animatedObject.GetComponent<CanvasGroup>();

        while (duration < time)
        {
            float t2 = Time.unscaledDeltaTime + duration / time > 1 ? 1 : Time.unscaledDeltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, endPosition, t2);
            canvasGroup.alpha = fadeType == Fade.In ? t2 : 1 - t2;
            duration += Time.unscaledDeltaTime;
            yield return null;
        }
        animatedObject.localPosition = endPosition;
        UtilsUI.ChangeVisibility(canvasGroup, fadeType == Fade.In);

        canAnimate = true;

        yield return null;
    }

    private IEnumerator MoveRectAndFade(float time, Transform animatedObject, Vector3 newPosition, Fade fadeType)
    {
        canAnimate = false;

        float duration = 0;
        Vector3 originalPosition = animatedObject.localPosition;
        Vector3 endPosition = newPosition;
        CanvasGroup canvasGroup = animatedObject.GetComponent<CanvasGroup>();
        if (!canvasGroup)
            canvasGroup = animatedObject.gameObject.AddComponent<CanvasGroup>();

        while (duration < time)
        {
            float t2 = Time.unscaledDeltaTime + duration / time > 1 ? 1 : Time.unscaledDeltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, endPosition, t2);
            canvasGroup.alpha = fadeType == Fade.In ? t2 : 1 - t2;
            duration += Time.unscaledDeltaTime;
            yield return null;
        }
        animatedObject.localPosition = endPosition;
        UtilsUI.ChangeVisibility(canvasGroup, fadeType == Fade.In);

        canAnimate = true;

        yield return null;
    }

    private IEnumerator FadeUIElement(CanvasGroup element, Fade fadeType, float fadeTime)
    {
        float duration = 0;

        while (duration < fadeTime && element != null)
        {
            float t2 = Time.unscaledDeltaTime + duration / fadeTime > 1 ? 1 : Time.unscaledDeltaTime + duration / fadeTime;

            element.alpha = fadeType == Fade.In ? t2 : 1 - t2;

            duration += Time.unscaledDeltaTime;
            yield return null;
        }
        if (element != null)
            UtilsUI.ChangeVisibility(element, fadeType == Fade.In);

        yield return null;
    }

    private IEnumerator ScaleUIElement(Transform element, Scale scaleType, float scaleTime)
    {
        float duration = 0;

        while (duration < scaleTime)
        {
            float t2 = Time.unscaledDeltaTime + duration / scaleTime > 1 ? 1 : Time.unscaledDeltaTime + duration / scaleTime;

            element.localScale = scaleType == Scale.Up ? new Vector3(t2, t2, 1) : new Vector3(1 - t2, 1 - t2, 1);

            duration += Time.unscaledDeltaTime;
            yield return null;
        }
        element.localScale = scaleType == Scale.Up ? Vector3.one : Vector3.zero;
        yield return null;
    }
}