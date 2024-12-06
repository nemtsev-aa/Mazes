using System;
using UnityEngine;

public enum SwipeDirection {
    Up,
    Down,
    Right,
    Left,
    None
}

public class SwipeHandler : IPause, IDisposable {
    public event Action<Vector3> PointerShow;
    public event Action<Vector3> PointerPositionChanged;
    public event Action<SwipeDirection> SwipeDirectionChanged;

    private readonly IInput _input;
    private bool _isPaused;

    public SwipeHandler(IInput input) {
        _input = input;

        _input.StartSwiping += OnStartSwiping;
        _input.ProgressSwiping += OnSwiping;
        _input.SwipeUp += OnSwipeUp;
        _input.SwipeDown += OnSwipeDown;
        _input.SwipeLeft += OnSwipeLeft;
        _input.SwipeRight += OnSwipeRight;
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;

    public void Dispose() {
        _input.StartSwiping -= OnStartSwiping;
        _input.ProgressSwiping -= OnSwiping;
        _input.SwipeUp -= OnSwipeUp;
        _input.SwipeDown -= OnSwipeDown;
        _input.SwipeLeft -= OnSwipeLeft;
        _input.SwipeRight -= OnSwipeRight;
    }

    private void OnSwipeUp() => SwipeDirectionChanged?.Invoke(SwipeDirection.Up);

    private void OnSwipeDown() => SwipeDirectionChanged?.Invoke(SwipeDirection.Down);

    private void OnSwipeLeft() => SwipeDirectionChanged?.Invoke(SwipeDirection.Left);

    private void OnSwipeRight() => SwipeDirectionChanged?.Invoke(SwipeDirection.Right);

    private void OnStartSwiping(Vector3 startPosition) => PointerShow?.Invoke(startPosition);

    private void OnSwiping(Vector3 mousePosition) => PointerPositionChanged?.Invoke(mousePosition);

}

