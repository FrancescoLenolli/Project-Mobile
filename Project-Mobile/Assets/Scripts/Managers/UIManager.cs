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

    // Set if a Canvas is visible or not based on his CanvasGroup.
    public void ChangeStatus(CanvasGroup canvasGroup, bool isVisible)
    {
        int newAlphaValue = isVisible ? 1 : 0;

        canvasGroup.alpha = newAlphaValue;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    // Increase size of a container to fit more elements
    public Vector2 ResizeContainer(Transform container, Transform additionalElement, float additionalSpace)
    {
        RectTransform additionalElementRect = additionalElement.GetComponent<RectTransform>();
        RectTransform containerRect = container.GetComponent<RectTransform>();

        Vector2 containerUpdatedSize = new Vector2(containerRect.sizeDelta.x, containerRect.sizeDelta.y + additionalElementRect.sizeDelta.y + additionalSpace);
        return containerUpdatedSize;
    }

    public void MoveObject(float time, Transform animatedObject, Vector3 newPosition)
    {
        if (canAnimate)
            StartCoroutine(MoveRectTransform(time, animatedObject, newPosition));
    }

    public void MoveObjectAndFade(float time, Transform animatedObject, Vector3 newPosition, Fade fadeType)
    {
        if (canAnimate)
            StartCoroutine(MoveRectAndFade(time, animatedObject, newPosition, fadeType));
    }

    public void ChangeAllButtons(List<Button> listButtons, bool isInteractable)
    {
        foreach(Button button in listButtons)
        {
            button.interactable = isInteractable;
        }
    }

    private IEnumerator MoveRectTransform(float time, Transform animatedObject, Vector3 newPosition)
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
        ChangeStatus(canvasGroup, fadeType == Fade.In ? true : false);

        canAnimate = true;

        yield return null;
    }
}
