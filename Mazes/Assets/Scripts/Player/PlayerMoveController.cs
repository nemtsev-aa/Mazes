using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour {
    public event Action MoveImpossible;
    public event Action MoveComplete;
    public event Action<MoveDirections> MoveDirectionChanged;
    public event Action<GameplayElement> GameplayElementCollected;
    public event Action ExitReached;

    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private Ease _currentEase = Ease.InFlash;
    [SerializeField] private GameObject _pointer;

    private Rigidbody _rigidbody;
    private IMoveInput _input;

    private MoveInputHandler _inputHandler;
    private GameplayElement _gameplayElement;
    
    private Vector3 _currentTargetPoint;


    public bool IsMove { get; private set; } = false;
    public Vector2Int CurrentPosition { get; private set; }


    public void Init(MoveInputHandler inputHandler) {
        _inputHandler = inputHandler;
        _inputHandler.MoveDirectionChanged += SetTargetPoint;
    }

    private void Start() {
        _rigidbody ??= GetComponent<Rigidbody>();
    }

    private void Update() {
        if (IsMove == false)
            TweenMove();
    }

    public void MoveToDoor(Transform doorTransform) {
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .SetDelay(2f)
            .Append(transform.DOMove(doorTransform.position, _moveSpeed)
                 .SetEase(_currentEase)
                 .OnComplete(() => ExitReached?.Invoke()));
    }

    private void SetTargetPoint(Vector3 targetPoint) {
        if (IsMove)
            return;

        _currentTargetPoint = transform.position + targetPoint;
    }
    
    private void TweenMove() {
        if (_currentTargetPoint != Vector3.zero) {
            if (CheckDirection(_currentTargetPoint) == false) {
                MoveImpossible?.Invoke();
                
                return;
            }
            else {
                if (_gameplayElement != null) {
                    transform.DOLookAt(_gameplayElement.transform.position, _moveSpeed / 2, AxisConstraint.Y);
                    GameplayElementCollected?.Invoke(_gameplayElement);
                    _currentTargetPoint = Vector3.zero;
                    
                    return;
                }
            }

            IsMove = true;

            CurrentPosition = new Vector2Int((int)_currentTargetPoint.x, (int)_currentTargetPoint.y);

            Sequence mySequence = DOTween.Sequence();
            mySequence
              .Append(transform.DOLookAt(_currentTargetPoint, _moveSpeed / 2, AxisConstraint.Y))
                     .SetEase(_currentEase)
              .Append(transform.DOMove(_currentTargetPoint, _moveSpeed)
                     .SetEase(_currentEase)
                     .OnComplete(MovingComplete));

            _currentTargetPoint = Vector3.zero;

            return;
        }

        IsMove = false;
    }

    private bool CheckDirection(Vector3 targetPoint) {

        Vector3 direction = targetPoint - transform.position;

        Ray ray = new Ray(transform.position, direction);
        Debug.DrawRay(transform.position, direction, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude)) {
            if (_pointer != null)
                Instantiate(_pointer, hit.point, Quaternion.identity);

            if (hit.collider.attachedRigidbody.TryGetComponent(out GameplayElement gameplayElement)) {
                _gameplayElement = gameplayElement;
                return true;
            }
            else
                return false;
        }

        return true;
    }

    private void MovingComplete() {
        IsMove = false;
        MoveComplete?.Invoke();
    }

}
