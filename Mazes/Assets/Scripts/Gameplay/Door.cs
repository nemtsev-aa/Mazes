using UnityEngine;

public class Door : GameplayElement {
    [SerializeField] private Animator _animator;

    public void Open() {
        _animator.SetTrigger("Open");
    }
}

