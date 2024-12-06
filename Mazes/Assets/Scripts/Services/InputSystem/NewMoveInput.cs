using System;
using UnityEngine;
using Zenject;

public class NewMoveInput : ITickable, IMoveInput {
    public event Action<MoveDirections> Moved;

    private const float DeadZone = 0.5f;

    public NewMoveInput() {
        _input = new PlayerInput();
    }

    private PlayerInput _input;
    private Vector2 _inputDelta;

    public void Enable() => _input.Enable();

    public void Disable() => _input.Disable();
    
    public void Tick() {
        ReadInput();
    }

    public void ReadInput() {
        _inputDelta = _input.Player.Move.ReadValue<Vector2>();

        if (_inputDelta == Vector2.zero)
            return;

        if (_inputDelta.magnitude > DeadZone) {
            if (Mathf.Abs(_inputDelta.x) > Mathf.Abs(_inputDelta.y)) {
                if (_inputDelta.x > 0)
                    Moved?.Invoke(MoveDirections.Right);
                else
                    Moved?.Invoke(MoveDirections.Left);
            }
            else {
                if (_inputDelta.y > 0)
                    Moved?.Invoke(MoveDirections.Up);
                else
                    Moved?.Invoke(MoveDirections.Down);
            }
        }
    }
}

