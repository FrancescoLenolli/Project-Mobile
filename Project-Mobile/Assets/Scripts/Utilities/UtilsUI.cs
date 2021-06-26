﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UtilsUI
{
    public enum Resize { Add, Subtract }
    public enum Cycle { Left, Right }

    /// <summary>
    /// Cycle through a list. When reaching one end of the list, stop.
    /// </summary>
    public static int CycleListIndexClosed(int currentIndex, int maxValue, Cycle cycleType)
    {
        return CycleListIndex(currentIndex, maxValue, cycleType, true);
    }

    /// <summary>
    /// Cycle through a list. When reaching one end of the list, goes back to the other end.
    /// </summary>
    public static int CycleListIndexOpen(int currentIndex, int maxValue, Cycle cycleType)
    {
        return CycleListIndex(currentIndex, maxValue, cycleType, false);
    }

    /// <summary>
    /// Given a list of Buttons, set them as Interactable or Not Interactable.
    /// </summary>
    public static void ChangeAllButtons(List<Button> listButtons, bool isInteractable)
    {
        foreach (Button button in listButtons)
        {
            button.interactable = isInteractable;
        }
    }

    /// <summary>
    /// Initialize starting Slider values.
    /// </summary>
    /// <param name="startValue"> Starting slider value. If not set, it will be equal to maxValue. </param>
    public static void InitSliderValues(Slider slider, float minValue, float maxValue, float startValue = float.MinValue)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = startValue != float.MinValue ? startValue : maxValue;
    }

    /// <summary>
    /// Increase or Decrease size of a container by additional element.
    /// </summary>
    public static void ResizeContainer(Transform transform, Transform container, Resize resizeType)
    {
        RectTransform rectTransform = container as RectTransform;
        rectTransform.sizeDelta = ResizeContainer(container, transform, 0, resizeType);
    }

    public static void ChangeVisibility(List<Transform> uiElements, int visibleElementIndex)
    {
        for (int i = 0; i < uiElements.Count; ++i)
        {
            if (i == visibleElementIndex)
                ChangeStatus(GetCanvasGroup(uiElements[i]), true);
            else
                ChangeStatus(GetCanvasGroup(uiElements[i]), false);
        }
    }

    /// <summary>
    /// Set a UI Element visible/hidden using a CanvasGroup Component. If missing, it will be added first.
    /// </summary>
    public static void ChangeVisibility(Transform uiElement, bool isVisible)
    {
        ChangeStatus(GetCanvasGroup(uiElement), isVisible);
    }

    public static void ChangeVisibility(List<Transform> uiElements, bool isVisible)
    {
        foreach (Transform element in uiElements)
        {
            ChangeStatus(GetCanvasGroup(element), isVisible);
        }
    }

    public static bool IsPointerOverUI()
    {
        // passing a value of 0 makes it work on mobile but not on pc, so I have to use both
        return EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject();
    }

    public static void ChangeStatus(CanvasGroup canvasGroup, bool isVisible)
    {
        int newAlphaValue = isVisible ? 1 : 0;

        canvasGroup.alpha = newAlphaValue;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    public static CanvasGroup GetCanvasGroup(Transform uiElement)
    {
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = uiElement.gameObject.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    private static Vector2 ResizeContainer(Transform container, Transform additionalElement, float bufferSpace = 0, Resize resizeType = Resize.Add)
    {
        RectTransform additionalElementRect = additionalElement as RectTransform;
        RectTransform containerRect = container as RectTransform;

        float newSizeY = resizeType == Resize.Add ? containerRect.sizeDelta.y + additionalElementRect.sizeDelta.y + bufferSpace : containerRect.sizeDelta.y - additionalElementRect.sizeDelta.y - bufferSpace;
        float newSizeX = containerRect.sizeDelta.x;

        Vector2 containerUpdatedSize = new Vector2(newSizeX, newSizeY);
        return containerUpdatedSize;
    }

    private static int CycleListIndex(int currentIndex, int maxValue, Cycle cycleType, bool isClosed)
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
}
