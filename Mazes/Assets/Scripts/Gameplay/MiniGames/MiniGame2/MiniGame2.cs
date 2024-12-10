using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class MiniGame2 : MiniGame {
    public event Action MiniGame2_IsStarted;

    [SerializeField] private DialInDisk _dialInDisk;
    [SerializeField] private PasswordPanel _passwordPanel;
    [SerializeField] private MiniGame2_Player _player;
    [SerializeField] private MiniGame1_SoundManager _soundManager;

    [SerializeField] private RectTransform _lockRect;
    [SerializeField] private TextMeshProUGUI _infoText;

    public override void Init(MiniGameConfig config) {
        base.Init(config);
    }

    public override void StartGame() {
        base.StartGame();

        _dialInDisk.Init();

        _player.Init(this);
        _player.MoveDone += OnPlayerMoveDone;
        _player.MoveError += OnPlayerMoveError;

        int challenge = (int)Config.DifficultyMode;
        _passwordPanel.Init(this, challenge, _dialInDisk.CharInDiskList);
        _passwordPanel.CharsIsOver += Success;

        SetNewGame();
    }

    private void OnPlayerMoveDone(int value) {
        bool result = _passwordPanel.CheckCharInPassword(value);

        if (result == false)
            FinishGame();
    }

    private void OnPlayerMoveError() {
        FinishGame();
    }

    private void Success() {
        IsSuccess = true;
        FinishGame();
    }

    public override void FinishGame() {
        IsGameOver = true;
        _passwordPanel.gameObject.SetActive(false);

        if (IsSuccess) {
            _soundManager.PlaySuccess();
            _infoText.text = $"{VICTORY}";

            _lockRect.DORotate(Vector3.forward * -30f, 1f)
                     .OnComplete(() => base.FinishGame());
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

        MiniGame2_IsStarted?.Invoke();
    }

    public override void Dispose() {
        base.Dispose();

        _player.MoveDone -= OnPlayerMoveDone;
        _passwordPanel.CharsIsOver -= Success;

    }
}
