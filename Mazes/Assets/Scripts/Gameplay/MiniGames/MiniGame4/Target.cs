using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour {
    public event Action<bool> TargetShowed;

    [SerializeField] private float _duration;

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody ??= GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;

    }

    public void Init() {
        Show(true);
    }

    public void Show(bool status) {
        Vector3 endScale = status ? Vector3.one : Vector3.zero;
        transform.DOScale(endScale, _duration)
                 .OnComplete(() => TargetShowed?.Invoke(status));
    }

}
