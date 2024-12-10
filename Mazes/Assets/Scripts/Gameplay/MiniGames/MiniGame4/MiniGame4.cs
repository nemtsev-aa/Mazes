using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class MiniGame4 : MiniGame {
    public event Action Started;
    public event Action<Target> CurrentTargetChanged;

    [SerializeField] private MiniGameConfig _currentConfig;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private Obstacles _obstacles;
    [SerializeField] private MiniGame4_Player _player;
    [SerializeField] private MiniGame1_SoundManager _soundManager;

    [SerializeField] private RectTransform _lockRect;
    [SerializeField] private TextMeshProUGUI _infoText;

    private int _numTotalOfTarget;

    public MiniGame4_Config Game_Config { get; private set; }

    [Inject]
    public void Construct(MiniGame4_Config game_Config) {
        Game_Config = game_Config;
    }

    private void Start() {
        Init(_currentConfig);
        StartGame();
    }

    public override void Init(MiniGameConfig config) {
        base.Init(config);
    }

    public override void StartGame() {
        base.StartGame();

        _obstacles.Init(this);

        _targetSpawner.Init(this);
        _targetSpawner.CurrentTargetChanged += OnCurrentTargetChanged;

        _numTotalOfTarget = Mathf.RoundToInt((int)Config.DifficultyMode * 1.5f);
        UpdateInfoText();

        _player.Init(this);
        _player.ObstacleHaBeenReached += OnObstacleHaBeenReached;

        SetNewGame();
    }

    private void OnCurrentTargetChanged(Target target) {
        CurrentTargetChanged?.Invoke(target);
    }

    private void UpdateInfoText() {
        if (_numTotalOfTarget > 0) {
            _infoText.fontSize = 25f;
            _infoText.text = $"{_numTotalOfTarget}";
            return;
        }

        _infoText.fontSize = 30f;
        if (IsSuccess)
            _infoText.text = $"{VICTORY}";
        else
            _infoText.text = $"{FAIL}";
    }

    private void OnObstacleHaBeenReached(bool result) {
        if (result == true) {
            _numTotalOfTarget = 0;
            FinishGame();
            return;
        }

        _numTotalOfTarget--;
        if (_numTotalOfTarget <= 0) {
            Success();
            return;
        }

        UpdateInfoText();
        _player.Activate();
        _soundManager.PlayTouch();
    }

    private void SetNewGame() {
        IsGameOver = false;
        IsStarted = true;

        Started?.Invoke();
    }

    private void Success() {
        IsSuccess = true;
        FinishGame();
    }

    public override void FinishGame() {
        IsGameOver = true;
        _player.gameObject.SetActive(false);
        
        UpdateInfoText();

        if (IsSuccess) {
            _soundManager.PlaySuccess();
            _lockRect.DORotate(Vector3.forward * -30f, 1f)
                     .OnComplete(() => base.FinishGame());
            return;
        }

        _soundManager.PlayFail();
        _lockRect.parent.DOScale(Vector3.one * 1.02f, 1f)
                 .OnComplete(() => base.FinishGame());
    }

    public override void Dispose() {
        base.Dispose();

        _player.ObstacleHaBeenReached -= OnObstacleHaBeenReached;
    }
}
