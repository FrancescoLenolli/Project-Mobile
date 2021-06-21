using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    public enum Swipe { Up, Down }

    public Action<Swipe, Vector2> EventSwipe;

    [SerializeField] private float minimumDistance = 10f;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private Action swipeDetection;

    private void Start()
    {
        ChooseSwipeDetectionMethod();
    }

    private void Update()
    {
        swipeDetection?.Invoke();
    }

    private void ChooseSwipeDetectionMethod()
    {
        if (Vibration.IsAndroid)
            swipeDetection = AndroidDetection;
        else
            swipeDetection = PCDetection;
    }

    private void AndroidDetection()
    {
        Touch touch;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = Input.mousePosition;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                endPosition = Input.mousePosition;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                MotionEnded();
            }
        }
    }

    private void PCDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            MotionEnded();
        }
    }

    private void MotionEnded()
    {
        endPosition = Input.mousePosition;

        CheckSwipe();

        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
    }

    private void CheckSwipe()
    {
        bool isSwipingDown = startPosition.y > endPosition.y && startPosition.y - endPosition.y >= minimumDistance;
        bool isSwipingUp = endPosition.y > startPosition.y && endPosition.y - startPosition.y >= minimumDistance;

        if (isSwipingDown)
        {
            EventSwipe?.Invoke(Swipe.Down, startPosition);
        }
        else if (isSwipingUp)
        {
            EventSwipe?.Invoke(Swipe.Up, startPosition);
        }
    }
}
