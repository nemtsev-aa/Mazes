using System;
using UnityEngine;

public class Player : MonoBehaviour, IDisposable {
    public event Action ExitReached;

    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private PlayerMoveController _moveController;
    [SerializeField] private PlayerView _view;
    [SerializeField] private PlayerUI _ui;

    public PlayerMoveController MoveController => _moveController;
    public PlayerView View => _view;
    public PlayerUI UI => _ui;
    public InteractionHandler InteractionHandler => _interactionHandler;

    public bool HasKey { get; private set; } = false;
    public HintRenderer HintRenderer { get; private set; }

    private MiniGameMediator _miniGameMediator;

    public void Init(MoveInputHandler inputHandler, HintRenderer hintRenderer, MiniGamesFactory miniGamesFactory) {
        HintRenderer = hintRenderer;
        
        _miniGameMediator = new MiniGameMediator(miniGamesFactory);
        _miniGameMediator.Init(this);

        _interactionHandler.Init(this, _miniGameMediator);
        _view.Init(_moveController);
        _ui.Init(this);
        _moveController.Init(inputHandler);

        AddListeners();
    }

    private void AddListeners() {
        _interactionHandler.KeyCollected += OnKeyCollected;
        _interactionHandler.ExitReached += OnExitReached;
    }

    private void RemoveListeners() {
        _interactionHandler.KeyCollected -= OnKeyCollected;
        _interactionHandler.ExitReached -= OnExitReached;
    }


    private void OnKeyCollected() {
        HasKey = true;
        _view.ShowHappy();

        Debug.Log($"Key Collected!");
    }

    private void OnExitReached() {
        Debug.Log("Opened the door");
    }

    public void Dispose() {
        RemoveListeners();
    }
}
