using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public enum Fade { In, Out }

    private bool canAnimate = true;

    private new void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Set if a Canvas is visible or not based on his CanvasGroup.
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="isVisible"></param>
    public void ChangeStatus(CanvasGroup canvasGroup, bool isVisible)
    {
        int newAlphaValue = isVisible ? 1 : 0;

        canvasGroup.alpha = newAlphaValue;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    /// <summary>
    /// Increase size of a container to fit more elements.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="additionalElement"></param>
    /// <param name="additionalSpace"></param>
    /// <returns></returns>
    public Vector2 ResizeContainer(Transform container, Transform additionalElement, float additionalSpace)
    {
        RectTransform additionalElementRect = additionalElement.GetComponent<RectTransform>();
        RectTransform containerRect = container.GetComponent<RectTransform>();

        Vector2 containerUpdatedSize = new Vector2(containerRect.sizeDelta.x, containerRect.sizeDelta.y + additionalElementRect.sizeDelta.y + additionalSpace);
        return containerUpdatedSize;
    }

    /// <summary>
    /// Move Canvas Element to newPosition in n seconds.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="animatedObject"></param>
    /// <param name="newPosition"></param>
    public void MoveRectObject(float time, Transform animatedObject, Vector3 newPosition)
    {
        if (canAnimate)
            StartCoroutine(MoveRect(time, animatedObject, newPosition));
    }

    public void MoveRectObject(float time, Transform animatedObject, Transform newPosition)
    {
        if (canAnimate)
            StartCoroutine(MoveRect(time, animatedObject, newPosition));
    }

    /// <summary>
    /// Move Canvas Element to newPosition in n seconds, with a Fade effect.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="animatedObject"></param>
    /// <param name="newPosition"></param>
    /// <param name="fadeType"> Should the object fade In or Out? </param>
    public void MoveRectObjectAndFade(float time, Transform animatedObject, Vector3 newPosition, Fade fadeType)
    {
        if (canAnimate)
            StartCoroutine(MoveRectAndFade(time, animatedObject, newPosition, fadeType));
    }

    public void MoveRectObjectAndFade(float time, Transform animatedObject, Transform newPosition, Fade fadeType)
    {
        if (canAnimate)
            StartCoroutine(MoveRectAndFade(time, animatedObject, newPosition, fadeType));
    }

    /// <summary>
    /// Given a list of Buttons, set them as Interactable or Not Interactable.
    /// </summary>
    /// <param name="listButtons"></param>
    /// <param name="isInteractable"> Set true to make buttons Interactable. </param>
    public void ChangeAllButtons(List<Button> listButtons, bool isInteractable)
    {
        foreach(Button button in listButtons)
        {
            button.interactable = isInteractable;
        }
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

    private IEnumerator MoveRectAndFade(float time, Transform animatedObject, Vector3 newPosition, Fade fadeType)
    {
        canAnimate = false;

        float duration = 0;
        Vector3 originalPosition = animatedObject.localPosition;
        CanvasGroup canvasGroup = animatedObject.GetComponent<CanvasGroup>();

        while (duration < time)
        {
            float t2 = Time.deltaTime + duration / time > 1 ? 1 : Time.deltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, newPosition, t2);
            canvasGroup.alpha = fadeType == Fade.In ? t2 : 1 - t2;
            duration += Time.deltaTime;
            yield return null;
        }
        animatedObject.localPosition = newPosition;
        ChangeStatus(canvasGroup, fadeType == Fade.In);

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
            float t2 = Time.deltaTime + duration / time > 1 ? 1 : Time.deltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, endPosition, t2);
            canvasGroup.alpha = fadeType == Fade.In ? t2 : 1 - t2;
            duration += Time.deltaTime;
            yield return null;
        }
        animatedObject.localPosition = endPosition;
        ChangeStatus(canvasGroup, fadeType == Fade.In);

        canAnimate = true;

        yield return null;
    }
}
