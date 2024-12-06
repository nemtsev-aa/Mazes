using DG.Tweening;
using System;
using UnityEngine;

public class Bomb : GameplayElement {
    [SerializeField] private ParticleSystem _showParticle;
    [SerializeField] private float _aninationDuration = 1f;
    
    private Tween _initTween;

    public override void Init() {

        _initTween = transform.DOJump(transform.position, 2f, 1, _aninationDuration)
                              .OnComplete(_showParticle.Stop);
    }

    public void Activate(Transform target, Action nextAction) {
        transform.DOPunchPosition(Vector3.forward, _aninationDuration)
            .OnComplete(() => {
                if (_showParticle != null)
                    _showParticle.Play();

                nextAction?.Invoke();
            });
    }

}
