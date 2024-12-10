using System;
using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class MiniGame4_Player : MonoBehaviour, IDisposable {
    private const KeyCode START_MOVE = KeyCode.Space;

    public event Action<bool> ObstacleHaBeenReached;

    [SerializeField] private TrailView _trailView;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _moveSpeed;

    private Rigidbody _rigidbody;
    private MiniGame4 _miniGame;
    private Target _target;
    private Sequence _showTrail;
    private Sequence _moveSequence;


    private Vector3 _defaulScale => new Vector3(0, 1, 1);
    private bool IsGameOver => _miniGame.IsGameOver;
    private bool IsSuccess => _miniGame.IsSuccess;

    public bool IsReady { get; private set; }

    private void Awake() {
        _rigidbody ??= GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void Init(MiniGame4 miniGame4) {
        _miniGame = miniGame4;
        _miniGame.Started += OnStarted;
        _miniGame.CurrentTargetChanged += OnCurrentTargetChanged;

        _trailView.Init(_miniGame);
    }

    public void Activate() {

    }

    private void OnStarted() {
        
    }

    private void Update() {

        if (Input.GetKeyDown(START_MOVE) && !IsGameOver && !IsSuccess) {
            MoveToTarget();
        }
    }


    private void OnCurrentTargetChanged(Target target) {
        _target = target;

        transform.LookAt(_target.transform, transform.up);

        _showTrail = DOTween.Sequence();
        _showTrail.SetDelay(0.3f)
                  .OnComplete(() => {
                    IsReady = true;
                  });       
    }

    private void MoveToTarget() {
        if (IsReady == false)
            return;

        Vector3 defaultPosition = transform.position;
        Vector3[] path = new Vector3[] { _target.transform.position, defaultPosition };
        
        _moveSequence = DOTween.Sequence();
        //_moveSequence.Append(transform.DOMove(_target.transform.position, _moveSpeed));
        _moveSequence.Append(transform.DOPath(path, _moveSpeed));
        _moveSequence.OnComplete(() => ObstacleHaBeenReached?.Invoke(false));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody.TryGetComponent(out Obstacle obstacle)) {
            if (_moveSequence.active)
                _moveSequence.Kill();

            ObstacleHaBeenReached?.Invoke(true);
        }
    }

    public void Dispose() {
        _miniGame.Started -= OnStarted;
        _miniGame.CurrentTargetChanged -= OnCurrentTargetChanged;
    }
}
