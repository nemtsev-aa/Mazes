using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    public event Action<Color> BarrierTriggered;

    [SerializeField] private Image _sprite;
    [SerializeField] private float _moveSpeed;

    [SerializeField] private ParticleSystem _destroySystem;
    private Tween _moveTween;

    public Color Color { get; private set; }

    public void Init(Color color) {
        Color = color;
        _sprite.color = Color;

        Show(false);
    }

    public void Show(bool status) {
        gameObject.SetActive(status);

        if (status) {
            transform.localScale = Vector3.one * 0.8f;
            transform.DOScale(Vector3.one, 1f).SetEase(Ease.InBounce);
        }
    }

    public void StartMove() {
        _moveTween = transform.DOMove(transform.position + transform.up * 100, _moveSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody.TryGetComponent(out Barrier barrier)) {
            
            _sprite.enabled = false;
            _moveTween.Kill();

            if (_destroySystem != null)
                ShowDestroyParticle();

            BarrierTriggered?.Invoke(barrier.Color);
        }
    }

    private void ShowDestroyParticle() {
        _destroySystem.Play();

        Sequence delay = DOTween.Sequence();
        delay.SetDelay(_destroySystem.main.duration)
             .OnComplete(() => {
                 _destroySystem.Stop();
             });
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 100f);
    }
}
