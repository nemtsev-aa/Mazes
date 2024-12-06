using System;
using UnityEngine;

public class MoveInputHandler : IPause, IDisposable {
    public event Action<Vector3> MoveDirectionChanged;

    private readonly IMoveInput _input;

    private bool _isPaused;
    private float _cellSize;

    public MoveInputHandler(IMoveInput input) {
        _input = input;

        SetPause(false);
        _input.Moved += OnMoved;
    }

    public void SetCellSize(float cellSize) {
        _cellSize = cellSize;
    }

    private void OnMoved(MoveDirections inputDirection) {
        if (_isPaused)
            return;

        Vector3 targetPoint = GetMoveDirection(inputDirection) * _cellSize;

        MoveDirectionChanged?.Invoke(targetPoint);
    }

    private Vector3 GetMoveDirection(MoveDirections direction) {
         switch (direction) {
            case MoveDirections.Up:
                return Vector3.forward;
 
            case MoveDirections.Down:
                return Vector3.back;

            case MoveDirections.Left:
                return Vector3.left;

            case MoveDirections.Right:
                return Vector3.right;

            case MoveDirections.None:
                return Vector2.zero;

            default:
                throw new ArgumentException($"Invalid MoveDirection: {direction}");
        }
    }

    public void SetPause(bool isPaused) {
        _isPaused = isPaused;

        if (_isPaused)
            _input.Disable();
        else
            _input.Enable();
    }

    public void Dispose() {
        _input.Moved-= OnMoved;
    }
}

