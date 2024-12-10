using System;
using System.Collections;
using UnityEngine;

public class MiniGame2_Player : MonoBehaviour {
    private const KeyCode STOP_MOVE = KeyCode.Space;

    public event Action<int> MoveDone;
    public event Action MoveError;

    [SerializeField] private Collider _collider;
    [SerializeField] private float _time = 3f;
    [SerializeField] private bool _firstMove;

    private int _direction = 1;
    private MiniGame2 _miniGame;
    private int _currentValue;

    private bool GameIsStarted => _miniGame.IsStarted;
    private bool IsGameOver => _miniGame.IsGameOver;
    private bool IsSuccess => _miniGame.IsSuccess;


    private void Awake() {
        _firstMove = true;
        _direction = 1;
    }

    public void Init(MiniGame2 miniGame) {
        _miniGame = miniGame;
        _miniGame.MiniGame2_IsStarted += SetDefaultPosition;
    }

    private void SetDefaultPosition() {
        transform.eulerAngles = Vector3.zero;
        //_collider.enabled = false;
    }

    private void Update() {

        _collider.enabled = true;

        if (!GameIsStarted || IsGameOver || IsSuccess) {
            _firstMove = true;
            StopAllCoroutines();

            return;
        }

        if (Input.GetKeyDown(STOP_MOVE) && !IsGameOver && !IsSuccess) {

            if (_firstMove) {

                _direction = UnityEngine.Random.Range(1, 9) > 5 ? -1 : 1;

                StartTheMove();
                _firstMove = false;

                return;
            }


            if (CheckCurrentValue()) {
                MoveDone?.Invoke(_currentValue);

                if (IsSuccess)
                    return;

                StartTheMove();
                return;
            }

            MoveError?.Invoke();
        }
    }

    private bool CheckCurrentValue() {
        RaycastHit hit;
        Rigidbody rb;
        Ray ray = new Ray(_collider.transform.parent.position, transform.up);

        if (Physics.Raycast(ray, out hit)) {
            rb = hit.collider.attachedRigidbody;

            if (rb != null && rb.TryGetComponent(out CharInDisk charIn)) {
                _currentValue = charIn.Parameters.Value;
                return true;
            }
        }

        _currentValue = 0;
        return false;
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
        //bool isAfter = false;

        while (!IsGameOver) {
            float t0 = transform.rotation.eulerAngles.z;
            float t1 = transform.rotation.eulerAngles.z + _direction * 360f;
            float timer = 0;

            while (timer <= _time) {
                timer += Time.deltaTime;
                transform.eulerAngles = Vector3.forward * Mathf.Lerp(t0, t1, timer / _time);

                if (IsGameOver)
                    break;

                yield return null;
            }
        }
    }

    public void Dispose() {
        _miniGame.MiniGame2_IsStarted -= SetDefaultPosition;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_collider.transform.position, transform.up * Vector3.Distance(_collider.transform.position, _collider.bounds.center));
    }
}
