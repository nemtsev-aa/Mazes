using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour {
    private Ease _ease = Ease.Linear;

    private Rigidbody _rigidbody;
    private Tween _rotation;
    private Obstacles _obstacles;
    private ObstacleRotationConfig _config;

    private bool _rotateDirection;
    private float _rotationDuration;

    private float RotationAngle {
        get {
            if (_rotateDirection)
                return 360;
            else
                return -360;
        }
    }

    private void Awake() {
        _rigidbody ??= GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void Init(Obstacles obstacles, ObstacleRotationConfig config) {
        _obstacles = obstacles;
        _config = config;

        _rotateDirection = _config.Direction;
        _rotationDuration = _config.Duration;

        _obstacles.RotationStatusChanged += OnRotationStatusChanged;
        _obstacles.RotationDirectionChanged += ChangeDirection;
    }

    private void OnRotationStatusChanged(bool status) {
        if (status == false) {
            StopRotate();
            return;
        }

        StartRotate();
    }

    private void StartRotate() {
        _rotation = transform.DORotate(transform.eulerAngles + Vector3.forward * RotationAngle, _rotationDuration, RotateMode.FastBeyond360)
                             .SetEase(_ease)
                             .OnComplete(() => ChangeDirection());
    }

    private void StopRotate() {
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
        _obstacles.RotationStatusChanged -= OnRotationStatusChanged;
        _obstacles.RotationDirectionChanged -= ChangeDirection;
    }
}
