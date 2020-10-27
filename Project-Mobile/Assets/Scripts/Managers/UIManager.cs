using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
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
        if(canAnimate)
        StartCoroutine(MoveRectTransform(time, animatedObject, newPosition));
    }

    private IEnumerator MoveRectTransform(float time, Transform animatedObject, Vector3 newPosition)
    {
        //float t = 0;
        //while(originalPosition != newPosition)
        //{
        //    float t2 = Time.deltaTime + t / time > 1 ? 1 : Time.deltaTime + t / time;
        //    transform.localPosition = Vector3.Lerp(originalPosition.localPosition, newPosition.localPosition, t2);
        //    t += Time.deltaTime;
        //    yield return null;
        //}
        canAnimate = false;

        float duration = 0;
        Vector3 originalPosition = animatedObject.localPosition;

        while(duration < time)
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
}
