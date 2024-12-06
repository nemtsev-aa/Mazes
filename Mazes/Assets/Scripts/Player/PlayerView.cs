using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour, IDisposable {
    private const string IDLE = "Idle";
    private const string INFO = "Info";
    private const string HAPPY = "IsHappy";
    
    private const string MELEE_STRIKE = "Melee_Strike";
    private const string RANGE_STRIKE = "Range_Strike";

    private PlayerMoveController _controller;
    private Animator _animator;

    public void Init(PlayerMoveController controler) {
        _animator ??= GetComponent<Animator>();
        _controller = controler;

        _controller.MoveComplete += OnMoveComplete;
        _controller.MoveImpossible += OnMoveError;
    }

    public void ShowHappy() {
        _animator.SetTrigger(HAPPY);
    }

    private void OnMoveComplete() {
        _animator.SetBool(IDLE, true);
    }

    private void OnMoveError() {
        _animator.SetBool(INFO, true);
    }

    public void Dispose() {
        if (_controller != null) {
            _controller.MoveComplete -= OnMoveComplete;
            _controller.MoveImpossible -= OnMoveError;
        }
    }
}