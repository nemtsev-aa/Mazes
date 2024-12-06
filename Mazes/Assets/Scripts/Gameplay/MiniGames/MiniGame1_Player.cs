using System.Collections;
using UnityEngine;

public class MiniGame1_Player : MiniGame1_Helper {
    private const KeyCode STOP_MOVE = KeyCode.Space;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _time = 3f;
    [SerializeField] private bool _firstMove;

    private int _direction = 1;

    public float Rotation => transform.eulerAngles.z;
    private Vector2 CurrentDotPosition => DotPosition.DotTransform.position;
    private bool GameIsStarted => MiniGame1_Mediator.GameIsStarted;
    private bool IsGameOver => MiniGame1_Mediator.IsGameOver;
    private bool IsSuccess => MiniGame1_Mediator.IsSuccess;

    private void Awake() {
        _firstMove = true;
        _direction = 1;
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
                MiniGame1_Mediator.MoveDone();

                DotPosition.DotTransform.localScale = Vector2.zero;

                if (IsSuccess)
                    return;

                DotPosition.SetNewPosition();

                StartTheMove(); 
                return;
            }

            MiniGame1_Mediator.GameOver();
        }
    }

    private bool CheckCurrentPlayerPosition() {
        return Vector2.Distance(CurrentDotPosition, _playerTransform.position) <= DotPosition.DotSize;
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
                        MiniGame1_Mediator.GameOver();

                    break;
                }

                if (IsGameOver)
                    break;

                yield return null;
            }
        }
    }
}
