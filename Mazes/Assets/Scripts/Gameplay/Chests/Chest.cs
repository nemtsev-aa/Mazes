using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent (typeof(Animator))]
public class Chest : GameplayElement {
    public const string OPEN = "Open";

    public event Action<GameplayElement> Opened;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _particlePlayDelayTime = 0.75f;
    [SerializeField] private float _contentShowedTime = 0.75f;

    private Animator _animator;
    
    public ChestConfig Config { get; private set; }
    
   
    private void Start() {
        _animator ??= GetComponent<Animator>();
    }

    public void Init(ChestConfig config) {
        Config = config;
    }

    public void Open() {
        _particleSystem.Play();
        _animator.SetTrigger(OPEN);
    }

    public void ShowContent() {
        GameplayElement newElement = Instantiate(Config.ContentConfig.Prefab, transform.position, transform.rotation);
        newElement.Init();
        newElement.transform.SetParent(transform.parent);

        Opened?.Invoke(this);
    }

    public void Disappear(Action nextAction) {
        Sequence disappear = DOTween.Sequence();
        disappear.Append(transform.DOLocalRotate(transform.eulerAngles + Vector3.up * 360, 1f))
            .Append(transform.DOMove(transform.position + Vector3.up, 0.5f))
            .Append(transform.DOShakePosition(1f, Vector3.one * 1.1f, fadeOut: true).OnComplete(() => nextAction?.Invoke()));
    }
}
