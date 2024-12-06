using UnityEngine;
using UnityEngine.UI;
using System;

public class HintPanel : MonoBehaviour, IDisposable {
    public event Action Activated;

    [SerializeField] private Button _action;
    [SerializeField] private GameplayElementsPanel _gameplayElements;

    [field: SerializeField] public ChargeIcon ChargeIcon { get; private set; }

    public void Init() {
        AddListeners();
    }

    public void Show(bool status) {
        gameObject.SetActive(status);
    }

    private void AddListeners() {
        _action.onClick.AddListener(ActionButtonClick);
    }

    private void RemoveListeners() {
        _action.onClick.RemoveListener(ActionButtonClick);
    }

    private void ActionButtonClick() {
        Activated?.Invoke();
    }

    public void Dispose() {
        RemoveListeners();
    }
}
