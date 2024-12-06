using DG.Tweening;
using System;
using UnityEngine;

public class Coins : GameplayElement, ICollected {
    [SerializeField] private ParticleSystem _showParticle;
    [SerializeField] private float _aninationDuration = 1f;
    private Tween _initTween;

    public override void Init() {
        if (_showParticle != null)
            _showParticle.Play();

        _initTween = transform.DOJump(transform.position, 2f, 1, _aninationDuration)
                              .OnComplete(_showParticle.Stop);
    }


    public void MoveToTarget(Transform moveToTarget, Action nextAction) {
        transform.DOJump(moveToTarget.transform.position, 3f, 1, 1f).OnComplete(() => nextAction?.Invoke());
    }
}
