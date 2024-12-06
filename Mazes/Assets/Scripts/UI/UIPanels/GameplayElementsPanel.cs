using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameplayElementsPanel : MonoBehaviour {
    public event Action<Vector2Int> PositionSelected;

    [SerializeField] private GameplayElementsConfig _config;
    [SerializeField] private GameplayElementView _gameplayElementViewPrefab;
    [SerializeField] private RectTransform _parent;

    [SerializeField] private List<GameplayElementView> _elementViews = new List<GameplayElementView>();
    [SerializeField] private List<GameplayElement> _gameplayElementList = new List<GameplayElement>();

    public void Init(IReadOnlyList<GameplayElement> gameplayElementList) {
        if (_gameplayElementList.Count > 0)
            _gameplayElementList.Clear();

        _gameplayElementList.AddRange(gameplayElementList);

        CreateGameplayElementViews();
    }

    public void Show(bool status) {
        gameObject.SetActive(status);
    }

    private void CreateGameplayElementViews() {
        if (_config == null)
            return;

        if (_elementViews.Count > 0)
            Reset();

        foreach (GameplayElement iElement in _gameplayElementList) {
            GameplayElementConfig config = GetConfigByGameplayElement(iElement);
            Vector2Int position = iElement.ParentMazeCell.GetPosition();

            GameplayElementView view = Instantiate(_gameplayElementViewPrefab, _parent);
            view.Init(config, position);
            view.Selected += OnSelected;

            _elementViews.Add(view);
        }
    }

    private void Reset() {
        foreach (var iView in _elementViews) {
            iView.Selected -= OnSelected;
            Destroy(iView.gameObject);
        }

        _elementViews.Clear();
    }

    private GameplayElementConfig GetConfigByGameplayElement(GameplayElement element) {
        if (element is Chest)
            return _config.ElementConfigs.FirstOrDefault(c => c.Type == GameplayElementTypes.Chest);

        if (element is Door)
            return _config.ElementConfigs.FirstOrDefault(c => c.Type == GameplayElementTypes.Door);

        if (element is Key)
            return _config.ElementConfigs.FirstOrDefault(c => c.Type == GameplayElementTypes.Key);

        return null;
    }


    private void OnSelected(Vector2Int position) {
        Show(false);
        PositionSelected?.Invoke(position);
    }
}
