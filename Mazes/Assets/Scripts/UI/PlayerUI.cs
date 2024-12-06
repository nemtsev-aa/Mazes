using UnityEngine;
using System;

public class PlayerUI : MonoBehaviour, IDisposable {
    public event Action HintShowClicked;

    [SerializeField] private GameplayElementsPanel _elementsPanel;
    [SerializeField] private HintPanel _hintPanel;
    [SerializeField] private RectTransform _miniGameParent;

    private Player _player;
    private float _currentChange;
    private bool _isChanged;

    public  RectTransform MiniGameParent => _miniGameParent;
    private PlayerMoveController _moveController => _player.MoveController;
    private HintRenderer HintRenderer => _player.HintRenderer;
    private MazeSpawner MazeSpawner => HintRenderer.MazeSpawner;
    private ChargeIcon ChargeIcon => _hintPanel.ChargeIcon;
    private float MaxChange => HintRenderer.ResetDelay;


    public void Init(Player player) {
        _player = player;

        _hintPanel.Init();

        AddListeners();
    }

    private void OnMazeHasBeenCreated() {
        _elementsPanel.Init(MazeSpawner.GameplayElements);
    }

    private void AddListeners() {
        _moveController.MoveComplete += OnMoveComplete;
        _moveController.MoveImpossible += OnMoveImpossible;

        _elementsPanel.PositionSelected += OnPositionSelected;
        _hintPanel.Activated += OnActivated;
        MazeSpawner.MazeHasBeenCreated += OnMazeHasBeenCreated;
        MazeSpawner.GameplayElementListChanged += OnMazeHasBeenCreated;

        HintRenderer.ResetTimeChanged += OnResetTimeChanged;
    }

    private void OnActivated() {
        _elementsPanel.Show(true);
        _hintPanel.Show(false);
    }

    private void RemoveListeners() {
        _moveController.MoveComplete -= OnMoveComplete;
        _moveController.MoveImpossible -= OnMoveImpossible;

        _elementsPanel.PositionSelected -= OnPositionSelected;
        _hintPanel.Activated -= OnActivated;
        MazeSpawner.MazeHasBeenCreated -= OnMazeHasBeenCreated;
        MazeSpawner.GameplayElementListChanged -= OnMazeHasBeenCreated;

        HintRenderer.ResetTimeChanged -= OnResetTimeChanged;
    }


    private void OnMoveComplete() {
        
    }

    private void OnMoveImpossible() {
       
    }

    private void OnPositionSelected(Vector2Int position) {
        if (HintRenderer.IsShowed == false) {
            _isChanged = true;
            _currentChange = 0f;
            ChargeIcon.StartCharge();

            HintRenderer.DrawPath(_moveController.CurrentPosition, position);
            HintShowClicked?.Invoke();
        }
    }

    private void OnResetTimeChanged(float time) {
        if (_isChanged) {
            _hintPanel.Show(true);
            _currentChange = MaxChange - time;

            ChargeIcon.SetChargeValue(_currentChange, MaxChange);

            if (_currentChange >= MaxChange) {
                _isChanged = false;
                ChargeIcon.StopCharge();
            }
        }
    }

    public void Dispose() {
        RemoveListeners();
    }
}
