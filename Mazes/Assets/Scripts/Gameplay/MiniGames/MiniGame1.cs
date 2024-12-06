using UnityEngine;
using System;

public class MiniGame1 : MiniGame {
    [SerializeField] private MiniGameNavigationPanel _navigationPanel;
    [SerializeField] private MiniGame1_Mediator _gameMediator;

	private void OnValidate() {
        if (_navigationPanel == null)
            throw new ArgumentNullException($"MiniGame1: MiniGameNavigationPanel not found!");
    }

    public override void Init(MiniGameConfig config) {
        base.Init(config);

    }

	public override void StartGame() {
        _navigationPanel.Init();
        _navigationPanel.Paused += OnNavigationPanelPaused;
        _navigationPanel.Finished += OnNavigationPanelFinished;

        base.StartGame();

        int challenge = (int)Config.DifficultyMode * 2;
        _gameMediator.StartNewLevel(challenge);
        _gameMediator.MiniGameFinished += OnMiniGameFinished;
    }

    private void OnMiniGameFinished(bool result) {
        Result = result;
         FinishGame();
    }

    private void OnNavigationPanelFinished(bool status) {
        Result = status;
        FinishGame();
    }

    private void OnNavigationPanelPaused() {
        IsActive = !IsActive;
    }

    public override void FinishGame() {
        base.FinishGame();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I))
            Debug.Log($"Game status: {IsActive}");
    }
}
