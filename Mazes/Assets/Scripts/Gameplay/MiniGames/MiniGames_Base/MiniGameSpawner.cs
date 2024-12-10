using System;
using UnityEngine;

public class MiniGameSpawner {
    public event Action<bool> ChestOpenMiniGameFinished;

    private InteractionHandler _interactionHandler;
    private PlayerUI _playerUI;
    private RectTransform _parent;
    private MiniGamesFactory _miniGamesFactory;
    private MiniGame _currentMiniGame;

    public MiniGameSpawner(MiniGamesFactory miniGamesFactory) {
        _miniGamesFactory = miniGamesFactory;
        _miniGamesFactory.Init();
    }

    public void Init(Player player) {
        _interactionHandler = player.InteractionHandler;
        _interactionHandler.ChestOpenMiniGameStarted += OnChestOpenMiniGameStarted;

        _parent = player.UI.MiniGameParent;
    }

    private void OnChestOpenMiniGameStarted(MiniGameParameters parameters) {
        // Организовать переключатель мини-игр (создавать новую, если нет старой (переиспользование)
        _currentMiniGame = _miniGamesFactory.Get(parameters, _parent);
        _currentMiniGame.Finished += OnCurrentMiniGameFinished;
        
        _currentMiniGame.StartGame();
    }

    private void OnCurrentMiniGameFinished(bool status) {
        _currentMiniGame.Finished -= OnCurrentMiniGameFinished;
        Debug.Log($"OnCurrentMiniGameFinished: {status}");

        ChestOpenMiniGameFinished?.Invoke(status);
        _currentMiniGame.Dispose();
    }
}

