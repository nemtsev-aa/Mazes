using System;
using UnityEngine;
using Zenject;

public class DesktopInput : ITickable, IInput {
    private const float DeadZone = 80;

    public event Action<Vector3> StartSwiping;
    public event Action<Vector3> ProgressSwiping;
    public event Action SwipeDown;
    public event Action SwipeUp;
    public event Action SwipeRight;
    public event Action SwipeLeft;
    
    private const int LeftMouseButton = 0;

    private bool _isSwiping;

    private Vector2 _swipeDelta;
    private Vector2 _startPosition;

    private Vector3 CurrentMousePosition => Input.mousePosition;

    public void Tick() {

        ProcessClickUp();

        ProcessClickDown();

        ProcessSwipe();
    }

    private void ProcessSwipe() {
        if (_isSwiping == false) 
            return;

        ProgressSwiping?.Invoke(CurrentMousePosition);

        CheckSwipe();
    }

    private void ProcessClickDown() {
        if (Input.GetMouseButtonDown(LeftMouseButton)) {
            _isSwiping = true;
            _startPosition = CurrentMousePosition;
            
            StartSwiping?.Invoke(_startPosition);
        }
    }

    private void ProcessClickUp() {
        if (Input.GetMouseButtonUp(LeftMouseButton)) 
            _isSwiping = false;  
    }

    private void CheckSwipe() {
        _swipeDelta = Vector2.zero;

        _swipeDelta = (Vector2)CurrentMousePosition - (Vector2)_startPosition;

        if (_swipeDelta.magnitude > DeadZone) {
            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y)) {
                if (_swipeDelta.x > 0)
                    SwipeRight?.Invoke();
                else
                    SwipeLeft?.Invoke();
            }
            else 
            {
                if (_swipeDelta.y > 0)
                    SwipeUp?.Invoke();
                else
                    SwipeDown?.Invoke();
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe() {
        _isSwiping = false;

        _startPosition =  Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
