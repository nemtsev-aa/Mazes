using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class MiniGame3 : MiniGame {
    public event Action Started;

    [SerializeField] private Barriers _barriers;
    [SerializeField] private MiniGame3_Player _player;
    [SerializeField] private MiniGame1_SoundManager _soundManager;

    [SerializeField] private RectTransform _lockRect;
    [SerializeField] private TextMeshProUGUI _infoText;
    
    private int _numTotalOfBullet;

    public MiniGame3_Config Game3_Config { get; private set; }

    [Inject]
    public void Construct(MiniGame3_Config game3_Config) {
        Game3_Config = game3_Config;
    }

    public override void Init(MiniGameConfig config) {
        base.Init(config);
    }

    public override void StartGame() {
        base.StartGame();

        _barriers.Init(this);

        _numTotalOfBullet = Mathf.RoundToInt((int)Config.DifficultyMode * 1.5f);
        UpdateInfoText();


        _player.Init(this, _numTotalOfBullet);
        _player.BarrierHaBeenReached += OnBarrierHasBeenReached;

        SetNewGame();
    }

    private void UpdateInfoText() {
        if (_numTotalOfBullet > 0) {
            _infoText.fontSize = 25f;
            _infoText.text = $"{_numTotalOfBullet}";
            return;
        }

        _infoText.fontSize = 30f;
        if (IsSuccess)
            _infoText.text = $"{VICTORY}";
        else
            _infoText.text = $"{FAIL}";
    }

    private void OnBarrierHasBeenReached(bool result) {
        if (result == false) {
            _numTotalOfBullet = 0;
            FinishGame();
            return;
        }

        _numTotalOfBullet--;
        if (_numTotalOfBullet <= 0) {
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
        _barriers.gameObject.SetActive(false);
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

        _player.BarrierHaBeenReached -= OnBarrierHasBeenReached;
    }
}


