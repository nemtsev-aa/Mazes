using System;
using UnityEngine;

public interface IInput {
    event Action<Vector3> StartSwiping;
    event Action<Vector3> ProgressSwiping;

    event Action SwipeDown;
    event Action SwipeUp;
    event Action SwipeRight;
    event Action SwipeLeft;
}
