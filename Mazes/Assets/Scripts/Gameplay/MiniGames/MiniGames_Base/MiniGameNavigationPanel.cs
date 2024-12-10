using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameNavigationPanel : MonoBehaviour, IDisposable {
    public event Action Paused;
    public event Action<bool> Finished;

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _trueFinish;
    [SerializeField] private Button _falseFinish;

    public void Init() {
        AddListeners();
    }

    private void AddListeners() {
        _pauseButton.onClick.AddListener(PauseButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);
        _trueFinish.onClick.AddListener(TrueFinishClick);
        _falseFinish.onClick.AddListener(FalseFinishClick);
    }

    private void RemoveListeners() {
        _pauseButton.onClick.RemoveListener(PauseButtonClick);
        _exitButton.onClick.RemoveListener(ExitButtonClick);
        _trueFinish.onClick.RemoveListener(TrueFinishClick);
        _falseFinish.onClick.RemoveListener(FalseFinishClick);
    }

    private void PauseButtonClick() {
        Paused?.Invoke();
    }

    private void ExitButtonClick() {
        Finished?.Invoke(false);
    }

    private void TrueFinishClick() {
        Finished?.Invoke(true);
    }

    private void FalseFinishClick() {
        Finished?.Invoke(false);
    }

    public void Dispose() {
        RemoveListeners();
    }
}

