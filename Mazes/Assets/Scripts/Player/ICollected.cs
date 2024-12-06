using System;
using UnityEngine;

public interface ICollected {
    public void MoveToTarget(Transform moveToTarget, Action nextAction);
}
