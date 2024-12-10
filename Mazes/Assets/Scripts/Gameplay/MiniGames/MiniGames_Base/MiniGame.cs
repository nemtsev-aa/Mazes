using System;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour, IDisposable {
    protected const string FAIL = "Неудача";
    protected const string VICTORY = "Успех";

    public event Action<bool> Finished;

    [SerializeField] protected MiniGameNavigationPanel _navigationPanel;

    public bool IsStarted { get; protected set; }
    public bool IsPaused { get; protected set; }
    public bool IsSuccess { get; protected set; }
    public bool IsGameOver { get; protected set; }

    public MiniGameConfig Config { get; private set; }
    
    private void OnValidate() {
        if (_navigationPanel == null)
            throw new ArgumentNullException($"MiniGame1: MiniGameNavigationPanel not found!");
    }

    public virtual void Init(MiniGameConfig config) {
        Config = config;

        _navigationPanel.Init();
        _navigationPanel.Paused += OnNavigationPanelPaused;
        _navigationPanel.Finished += OnNavigationPanelFinished;
    }

    public virtual void StartGame() {
        IsStarted = true;
    }

    public virtual void FinishGame() {
        gameObject.SetActive(false);
        Finished?.Invoke(IsSuccess);
    }

    private void OnNavigationPanelFinished(bool status) {
        IsSuccess = status;
        FinishGame();
    }

    private void OnNavigationPanelPaused() {
        IsPaused = !IsPaused;
    }

    public virtual void Dispose() {
        _navigationPanel.Paused -= OnNavigationPanelPaused;
        _navigationPanel.Finished -= OnNavigationPanelFinished;

        Destroy(gameObject);
    }
}

