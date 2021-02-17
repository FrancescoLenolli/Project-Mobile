using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public enum Fade { In, Out }
    public enum Resize { Add, Subtract }
    public enum Scale { Up, Down }
    public enum Cycle { Left, Right }

    private bool canAnimate = true;


    private new void Awake()
    {
        base.Awake();
    }


    /// <summary>
    /// Set a UI Element visible/hidden using a CanvasGroup Component. If missing, it will be added first.
    /// </summary>
    public void ChangeVisibility(Transform uiElement, bool isVisible)
    {
        ChangeStatus(GetCanvasGroup(uiElement), isVisible);
    }

    public void ChangeVisibility(List<Transform> uiElements, int visibleElementIndex)
    {
        for(int i = 0; i < uiElements.Count; ++i)
        {
            if (i == visibleElementIndex)
                ChangeStatus(GetCanvasGroup(uiElements[i]), true);
            else
                ChangeStatus(GetCanvasGroup(uiElements[i]), false);
        }
    }

    /// <summary>
    /// Increase or Decrease size of a container by additional element.
    /// </summary>
    public void ResizeContainer(Transform transform, Transform container, UIManager.Resize resizeType)
    {
        RectTransform rectTransform = container as RectTransform;
        rectTransform.sizeDelta = ResizeContainer(container, transform, 0, resizeType);
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

    /// <summary>
    /// Given a list of Buttons, set them as Interactable or Not Interactable.
    /// </summary>
    public void ChangeAllButtons(List<Button> listButtons, bool isInteractable)
    {
        foreach (Button button in listButtons)
        {
            button.interactable = isInteractable;
        }
    }

    /// <summary>
    /// Cycle through a list. When reaching one end of the list, stop.
    /// </summary>
    public int CycleListIndexClosed(int currentIndex, int maxValue, Cycle cycleType)
    {
        return CycleListIndex(currentIndex, maxValue, cycleType, true);
    }

    /// <summary>
    /// Cycle through a list. When reaching one end of the list, goes back to the other end.
    /// </summary>
    public int CycleListIndexOpen(int currentIndex, int maxValue, Cycle cycleType)
    {
        return CycleListIndex(currentIndex, maxValue, cycleType, false);
    }

    /// <summary>
    /// Initialize starting Slider values.
    /// </summary>
    /// <param name="startValue"> Starting slider value. If not set, it will be equal to maxValue. </param>
    public void InitSliderValues(Slider slider, float minValue, float maxValue, float startValue = float.MinValue)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = startValue != float.MinValue ? startValue : maxValue;
    }

    private int CycleListIndex(int currentIndex, int maxValue, Cycle cycleType, bool isClosed)
    {
        int index = currentIndex;

        switch (cycleType)
        {
            case Cycle.Left:
                --index;
                if (index < 0)
                {
                    if (isClosed)
                        index = 0;
                    else
                        index = maxValue - 1;
                }
                break;

            case Cycle.Right:
                ++index;
                if (index > maxValue - 1)
                {
                    if (isClosed)
                        index = maxValue - 1;
                    else
                        index = 0;
                }
                break;

            default:
                break;
        }

        return index;
    }

    private Vector2 ResizeContainer(Transform container, Transform additionalElement, float bufferSpace = 0, Resize resizeType = Resize.Add)
    {
        RectTransform additionalElementRect = additionalElement as RectTransform;
        RectTransform containerRect = container as RectTransform;

        float newSizeY = resizeType == Resize.Add ? containerRect.sizeDelta.y + additionalElementRect.sizeDelta.y + bufferSpace : containerRect.sizeDelta.y - additionalElementRect.sizeDelta.y - bufferSpace;
        float newSizeX = containerRect.sizeDelta.x;

        Vector2 containerUpdatedSize = new Vector2(newSizeX, newSizeY);
        return containerUpdatedSize;
    }

    private CanvasGroup GetCanvasGroup(RectTransform uiElement)
    {
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = uiElement.gameObject.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    private CanvasGroup GetCanvasGroup(Transform uiElement)
    {
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = uiElement.gameObject.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    private void ChangeStatus(CanvasGroup canvasGroup, bool isVisible)
    {
        int newAlphaValue = isVisible ? 1 : 0;

        canvasGroup.alpha = newAlphaValue;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
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
        ChangeStatus(canvasGroup, fadeType == Fade.In);

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

        while (duration < time)
        {
            float t2 = Time.unscaledDeltaTime + duration / time > 1 ? 1 : Time.unscaledDeltaTime + duration / time;
            animatedObject.localPosition = Vector3.Lerp(originalPosition, endPosition, t2);
            canvasGroup.alpha = fadeType == Fade.In ? t2 : 1 - t2;
            duration += Time.unscaledDeltaTime;
            yield return null;
        }
        animatedObject.localPosition = endPosition;
        ChangeStatus(canvasGroup, fadeType == Fade.In);

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
            ChangeStatus(element, fadeType == Fade.In);

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