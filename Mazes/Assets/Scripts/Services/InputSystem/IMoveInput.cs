using System;

public interface IMoveInput {
    event Action<MoveDirections> Moved;

    void Enable();
    void Disable();
    void ReadInput();
}