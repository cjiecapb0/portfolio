using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public enum SwipeType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public delegate void OnSwipeInput(SwipeType type);
    public  event OnSwipeInput SwipeEvent;

    private bool isDragging, isMobilePlatform;
    private Vector2 tapPoint, swipeDelta;
    private float minSwipeDelta = 130f;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STABDALONE
        isMobilePlatform = false;
#else
                isMobilePlatform = true;
#endif
    }
    private void Update()
    {
        if (!isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                tapPoint = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    isDragging = true;
                    tapPoint = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled ||
                    Input.touches[0].phase == TouchPhase.Ended)
                    ResetSwipe();
            }
        }
        CalculateSwipe();
    }

    private void CalculateSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isDragging)
        {
            if (!isMobilePlatform && Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - tapPoint;
            else if (Input.touchCount > 0)
                swipeDelta = Input.touches[0].position - tapPoint;
        }
        if (swipeDelta.magnitude > minSwipeDelta)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    SwipeEvent(swipeDelta.x < 0 ? SwipeType.LEFT : SwipeType.RIGHT);
                else
                    SwipeEvent(swipeDelta.y > 0 ? SwipeType.UP : SwipeType.DOWN);
            }
            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        isDragging = false;
        tapPoint = swipeDelta = Vector2.zero;
    }
}
