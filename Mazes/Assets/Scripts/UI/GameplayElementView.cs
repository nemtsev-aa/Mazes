using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameplayElementView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public event Action<Vector2Int> Selected;

    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;

    [SerializeField] private Color _enterColor = Color.green;
    [SerializeField] private Color _exitColor = Color.white;

    private GameplayElementConfig _config;
    private Vector2Int _position;

    public void Init(GameplayElementConfig config, Vector2Int position) {
        _config = config;
        _position = position;

        _icon.sprite = _config.Icon;
        _background.sprite = _config.Background;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Selected?.Invoke(_position);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _background.color = _enterColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _background.color = _exitColor;
    }

}
