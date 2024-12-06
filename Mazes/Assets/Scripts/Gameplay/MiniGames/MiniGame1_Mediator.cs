using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class MiniGame1_Mediator : MiniGame1_Helper {
    private const string FAIL = "Неудача";
    private const string VICTORY = "Успех";

    public event Action<bool> MiniGameFinished;

    [SerializeField] private RectTransform _lockRect;
    [SerializeField] private TextMeshProUGUI _infoText;

    private int _numTotalOfMove;

    public bool GameIsStarted { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsSuccess { get; private set; }

    public void StartNewLevel(int numTotalOfMove) {
        _numTotalOfMove = numTotalOfMove;
        _infoText.text = $"{_numTotalOfMove}";

        SetNewGame();
    }

    private void SetNewGame() {
        IsGameOver = false;
        GameIsStarted = true;

        _lockRect.eulerAngles = Vector3.zero;
        MiniGame1_Player.transform.eulerAngles = Vector3.zero;

        DotPosition.SetNewPosition();
    }

    public void MoveDone() {
        _numTotalOfMove--;

        _infoText.text = $"{_numTotalOfMove}";

        if (_numTotalOfMove <= 0)
            Success();
        else
            SoundManager.PlayTouch();
    }

    public void GameOver() {
        SoundManager.PlayFail();
        IsGameOver = true;

        _infoText.text = $"{FAIL}";
        _lockRect.parent.DOScale(Vector3.one * 1.02f, 1f)
                 .OnComplete(() => MiniGameFinished?.Invoke(false));

    }

    private void Success() {
        IsSuccess = true;

        SoundManager.PlaySuccess();
        
        _infoText.text = $"{VICTORY}";
        _lockRect.DORotate(Vector3.forward * -30f, 1f)
                 .OnComplete(() => MiniGameFinished?.Invoke(true));
    }
}
