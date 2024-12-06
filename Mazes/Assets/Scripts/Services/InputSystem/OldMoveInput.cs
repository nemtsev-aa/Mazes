using System;
using UnityEngine;
using Zenject;

public class OldMoveInput : ITickable, IMoveInput {
    public event Action<MoveDirections> Moved;

    private const KeyCode UP_KEY = KeyCode.UpArrow;
    private const KeyCode DOWN_KEY = KeyCode.DownArrow;
    private const KeyCode LEFT_KEY = KeyCode.LeftArrow;
    private const KeyCode RIGHT_KEY = KeyCode.RightArrow;

    public bool IsActive { get; private set; }

    public void Enable() => IsActive = true;

    public void Disable() => IsActive = false;
    
    public void Tick() {
        ReadInput();
    }

    public void ReadInput() {
        if (IsActive == false)
            return;

        if (Input.GetKey(UP_KEY)) 
            Moved?.Invoke(MoveDirections.Up);
        
        if (Input.GetKey(DOWN_KEY)) 
            Moved?.Invoke(MoveDirections.Down);
        
        if (Input.GetKey(LEFT_KEY)) 
            Moved?.Invoke(MoveDirections.Left);
        
        if (Input.GetKey(RIGHT_KEY)) 
            Moved?.Invoke(MoveDirections.Right);
    }
}

