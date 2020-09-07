using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private new void Awake()
    {
        base.Awake();
    }

    // Set if a Canvas is visible or not based on his CanvasGroup.
    private void ChangeStatus(CanvasGroup canvasGroup, bool isVisible)
    {
        int alphaValue = isVisible ? 1 : 0;

        canvasGroup.alpha = alphaValue;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    public Vector2 ResizeContainer(Transform container, Transform additionalElement, float additionalSpace)
    {
        RectTransform additionalElementRect = additionalElement.GetComponent<RectTransform>();
        RectTransform containerRect = container.GetComponent<RectTransform>();

        Vector2 containerUpdatedSize = new Vector2(containerRect.sizeDelta.x, containerRect.sizeDelta.y + additionalElementRect.sizeDelta.y + additionalSpace);
        return containerUpdatedSize;
    }
}
