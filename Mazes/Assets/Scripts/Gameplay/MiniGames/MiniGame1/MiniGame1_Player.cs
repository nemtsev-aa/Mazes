using System;
using System.Collections;
using UnityEngine;

public class MiniGame1_Player : MonoBehaviour, IDisposable {
    private const KeyCode STOP_MOVE = KeyCode.Space;

    public event Action MoveDone;
    public event Action MoveError;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _time = 3f;
    [SerializeField] private bool _firstMove;

    private int _direction = 1;
    private MiniGame1 _miniGame1;

    public float Rotation => transform.eulerAngles.z;
    
    private MiniGame1_DotPosition DotPosition => _miniGame1.DotPosition;
    private Vector2 CurrentDotPosition => DotPosition.Transform.position;
    private bool GameIsStarted => _miniGame1.IsStarted;
    private bool IsGameOver => _miniGame1.IsGameOver;
    private bool IsSuccess => _miniGame1.IsSuccess;

    
    private void Awake() {
        _firstMove = true;
        _direction = 1;
    }

    public void Init(MiniGame1 miniGame1) {
        _miniGame1 = miniGame1;
        _miniGame1.MiniGame1_IsStarted += SetDefaultPosition;
    }

    private void SetDefaultPosition() {
        transform.eulerAngles = Vector3.zero;
    }

    private void Update() {
        if (!GameIsStarted || IsGameOver || IsSuccess) {
            _firstMove = true;
            StopAllCoroutines();

            return;
        }

        if (Input.GetKeyDown(STOP_MOVE) && !IsGameOver && !IsSuccess) {

            if (_firstMove) {
                _direction = DotPosition.IsLeftOfScreen ? -1 : 1;

                StartTheMove();
                _firstMove = false;

                return;
            }

            if (CheckCurrentPlayerPosition()) {
                MoveDone?.Invoke();

                DotPosition.Transform.localScale = Vector2.zero;

                if (IsSuccess)
                    return;

                DotPosition.SetNewPosition();

                StartTheMove(); 
                return;
            }

            MoveError?.Invoke();
        }
    }

    private bool CheckCurrentPlayerPosition() {
        return Vector2.Distance(CurrentDotPosition, _playerTransform.position) <= DotPosition.Size;
    }

    private void StartTheMove() {

        if (IsSuccess) {
            StopAllCoroutines();
            return;
        }

        _direction *= -1;

        StopAllCoroutines();
        StartCoroutine(_StartTheMove());
    }

    private IEnumerator _StartTheMove() {
        bool isAfter = false;

        while (!IsGameOver) {
            float t0 = transform.rotation.eulerAngles.z;
            float t1 = transform.rotation.eulerAngles.z + _direction * 360f;
            float timer = 0;

            while (timer <= _time) {
                timer += Time.deltaTime;
                transform.eulerAngles = Vector3.forward * Mathf.Lerp(t0, t1, timer / _time);

                if (!isAfter && CheckCurrentPlayerPosition())
                    isAfter = true;


                if (isAfter && !CheckCurrentPlayerPosition()) {
                    if (!IsSuccess)
                        MoveError?.Invoke();

                    break;
                }

                if (IsGameOver)
                    break;

                yield return null;
            }
        }
    }

    public void Dispose() {
        _miniGame1.MiniGame1_IsStarted -= SetDefaultPosition;
    }
}
