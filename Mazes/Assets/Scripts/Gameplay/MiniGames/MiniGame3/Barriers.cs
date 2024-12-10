using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Barriers : MonoBehaviour, IDisposable {
    [SerializeField] private List<Barrier> _barrierList;
    [SerializeField] private Ease _ease = Ease.Linear;

    private bool _rotateDirection = false;
    private MiniGame3 _miniGame3;
    private Tween _rotation;

    private List<Color> _barrierColors => _miniGame3.Game3_Config.Colors;
    private float _duration => _miniGame3.Game3_Config.BarriesRotationSpeed;

    private float RotationAngle {
        get {
            if (_rotateDirection)
                return 360;
            else
                return -360;
        }
    }

    public void Init(MiniGame3 miniGame3) {
        _miniGame3 = miniGame3;
        _miniGame3.Started += StartRotate;
        _miniGame3.Finished += StopRotate;

        BarriersInit();
    }

    private void BarriersInit() {
        if (_barrierList.Count > _barrierColors.Count)
            throw new ArgumentException($"Barriers: Not Enough Colors");

        for (int i = 0; i < _barrierList.Count; i++) {
            _barrierList[i].Init(_barrierColors[i]);
        }
    }

    private void StartRotate() {
        _rotation = transform.DORotate(transform.eulerAngles + Vector3.forward * RotationAngle, _duration, RotateMode.FastBeyond360)
                             .SetEase(_ease)
                             .OnComplete(() => ChangeDirection());
    }

    private void StopRotate(bool status) {
        if (_rotation.IsPlaying())
            _rotation.Kill();

        transform.eulerAngles = Vector3.zero;
    }

    private void ChangeDirection() {
        _rotation.Kill();
        _rotateDirection = !_rotateDirection;

        StartRotate();
    }

    public void Dispose() {
        _miniGame3.Started -= StartRotate;
        _miniGame3.Finished -= StopRotate;
    }
}
