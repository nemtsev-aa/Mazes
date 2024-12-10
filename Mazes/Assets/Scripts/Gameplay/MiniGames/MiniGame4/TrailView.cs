using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrailView : MonoBehaviour, IDisposable {
    private MiniGame4 _miniGame;
    private Tween _spriteScaler;

    [SerializeField] private float _scaleSpeed = 0.3f;

    [field: SerializeField] public Image Sprite { get; private set; }

    public void Init(MiniGame4 miniGame) {
        _miniGame = miniGame;

        _miniGame.Started += OnGameStarted;
        _miniGame.CurrentTargetChanged += OnCurrentTargetChanged;
    }

    private void OnGameStarted() {
        Show(false);
    }

    private void OnCurrentTargetChanged(Target target) {
        Show(true);
    }

    private void Show(bool status) {
        float endScale = status == true ? 1f : 0f;

        _spriteScaler = Sprite.transform.DOScaleX(endScale, _scaleSpeed);
    }

    public void Dispose() {
        _miniGame.Started -= OnGameStarted;
        _miniGame.CurrentTargetChanged -= OnCurrentTargetChanged;
    }
}
