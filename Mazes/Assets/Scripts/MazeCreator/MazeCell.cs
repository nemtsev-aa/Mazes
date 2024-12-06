using System.Collections.Generic;
using UnityEngine;

public enum AvailableNeighborPositions {
    Left,
    Right, 
    Bottom,
    Top,
    None
}

public class MazeCell {
    public int X;
    public int Y;

    public bool WallLeft = true;
    public bool WallBottom = true;

    public bool Visited = false;
    public int DistanceFromStart;
    public List<AvailableNeighborPositions> AvailableNeighbors { get; private set; } 

    public Vector2Int GetPosition() {
        return new Vector2Int(X, Y);
    }

    public AvailableNeighborPositions GetAvailableNeighbor() {
        if (AvailableNeighbors.Count > 0)
            return AvailableNeighbors[0];
        else
            return AvailableNeighborPositions.None;
    }

    public void SetAvailableNeighbors(List<AvailableNeighborPositions> list) {
        AvailableNeighbors = list;
    }
}
