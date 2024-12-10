using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class MiniGame1 : MiniGame {
    private const string FAIL = "Неудача";
    private const string VICTORY = "Успех";

    public event Action MiniGame1_IsStarted;
    public event Action<bool> MiniGame1_IsPaused;

    [SerializeField] private MiniGame1_SoundManager _soundManager;
    [SerializeField] private MiniGame1_Player _player;
    [SerializeField] private MiniGame1_DotPosition _dotPosition;

    [SerializeField] private RectTransform _lockRect;
    [SerializeField] private TextMeshProUGUI _infoText;

    private int _numTotalOfMove;

    public MiniGame1_Player Player => _player;
    public MiniGame1_DotPosition DotPosition => _dotPosition;
    private Vector2 CurrentDotPosition => DotPosition.Transform.position;

    private void Awake() {
        _player ??= FindObjectOfType<MiniGame1_Player>();
        _player.Init(this);
        _player.MoveDone += OnPlayerMoveDone;
        _player.MoveError += FinishGame;

        _soundManager ??= FindAnyObjectByType<MiniGame1_SoundManager>();
        _dotPosition ??= FindAnyObjectByType<MiniGame1_DotPosition>();
        _dotPosition.Init(this);
    }

    public override void Init(MiniGameConfig config) {
        base.Init(config);
    }

    public override void StartGame() {
        base.StartGame();

        int challenge = (int)Config.DifficultyMode * 2;
        _numTotalOfMove = challenge;
        _infoText.text = $"{_numTotalOfMove}";

        SetNewGame();
    }

    public override void FinishGame() {
        IsGameOver = true;

        if (IsSuccess) {
            _soundManager.PlaySuccess();
            _infoText.text = $"{VICTORY}";
            base.FinishGame();

            return;
        }

        _soundManager.PlayFail();
        _infoText.text = $"{FAIL}";
        _lockRect.parent.DOScale(Vector3.one * 1.02f, 1f)
                 .OnComplete(() => base.FinishGame());

    }

    private void SetNewGame() {
        IsGameOver = false;
        IsStarted = true;

        _lockRect.eulerAngles = Vector3.zero;

        MiniGame1_IsStarted?.Invoke();
    }

    private void OnPlayerMoveDone() {
        _numTotalOfMove--;

        _infoText.text = $"{_numTotalOfMove}";

        if (_numTotalOfMove <= 0) {
            Success();
            return;
        }
        
        _soundManager.PlayTouch();
    }

    private void Success() {
        IsSuccess = true;

        _lockRect.DORotate(Vector3.forward * -30f, 1f)
                 .OnComplete(() => FinishGame());
    }

    public override void Dispose() {
        base.Dispose();

        if (_player != null) {
            _player.MoveDone -= OnPlayerMoveDone; 
            _player.MoveError -= FinishGame;
        }
    }
}
