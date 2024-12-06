using System;
using System.Collections.Generic;
using UnityEngine;

public class Maze {
    public Maze(MazeCell[,] cells, Vector2Int finishPosition) {
        Cells = cells;
        FinishPosition = finishPosition;
    }

    public MazeCell[,] Cells { get; private set; }
    public Vector2Int FinishPosition { get; private set; }

    public MazeCell GetCellByPosition(int xPosition, int yPosition) {
        int width = Cells.GetLength(0);
        int height = Cells.GetLength(1);

        for (int x = 0; x < width - 1; x++) {
            for (int y = 0; y < height - 1; y++) {
                MazeCell currentCell = Cells[x, y];
                
                if (currentCell.X == xPosition && currentCell.Y == yPosition) 
                    return currentCell;
                
            }
        }

        return Cells[0,0];
    }

    public List<MazeCell> FindDeadEnds() {
        List<MazeCell> deadEnds = new List<MazeCell>();
        int width = Cells.GetLength(0);
        int height = Cells.GetLength(1);

        for (int x = 0; x < width - 1; x++) {
            for (int y = 0; y < height - 1; y++) {

                MazeCell currentCell = Cells[x, y];
                int neighbors = FindNeighborsCount(currentCell);

                if (neighbors == 1 && currentCell.DistanceFromStart > 0) {
                    deadEnds.Add(currentCell);
                    //Debug.Log($"{currentCell.GetPosition()}: {neighbors} neighbors");
                }
            }
        }

        return deadEnds;
    }

    public int FindNeighborsCount(MazeCell mazeCell) {
        int neighbors = 0;
        List<AvailableNeighborPositions> neighborPositions = new List<AvailableNeighborPositions>();

        // Проверка левого соседа
        bool leftNeighborAccess = CheckLeftNeighbor(mazeCell);
        if (leftNeighborAccess) {
            neighbors++;
            neighborPositions.Add(AvailableNeighborPositions.Left);
            //Debug.Log($"leftNeighbor - {leftNeighborAccess}");
        }

        // Проверка правого соседа
        bool rightNeighborAccess = CheckRightNeighbor(mazeCell);
        if (rightNeighborAccess) {
            neighbors++;
            neighborPositions.Add(AvailableNeighborPositions.Right);
            //Debug.Log($"rightNeighbor - {rightNeighborAccess}");
        }
            
        // Проверка верхнего соседа
        bool upNeighborAccess = CheckUpNeighbor(mazeCell);
        if (upNeighborAccess) {
            neighbors++;
            neighborPositions.Add(AvailableNeighborPositions.Top);
            //Debug.Log($"upNeighbor - {upNeighborAccess}");
        }

        // Проверка нижнего соседа (симметрично верхнему)
        bool downNeighborAccess = CheckDownNeighbor(mazeCell);
        if (downNeighborAccess) {
            neighbors++;
            neighborPositions.Add(AvailableNeighborPositions.Bottom);
            //Debug.Log($"downNeighbor - {downNeighborAccess}");
        }

        mazeCell.SetAvailableNeighbors(neighborPositions);
        return neighbors;
    }

    private bool CheckDownNeighbor(MazeCell mazeCell) {
        if (mazeCell.Y <= 0)
            return false;
        else if (!mazeCell.WallBottom)
            return true;
        else
            return false;
    }

    private bool CheckUpNeighbor(MazeCell mazeCell) {
        int height = Cells.GetLength(1);

        if (mazeCell.Y >= height)
            return false;
        else if (!Cells[mazeCell.X, mazeCell.Y + 1].WallBottom)
            return true;
        else
            return false;
    }

    private bool CheckLeftNeighbor(MazeCell mazeCell) {
        if (mazeCell.X == 0)
            return false;
        else if (mazeCell.X > 0 && !mazeCell.WallLeft)
            return true;
        else
            return false;
    }

    private bool CheckRightNeighbor(MazeCell mazeCell) {
        int width = Cells.GetLength(0);

        if (mazeCell.X >= width - 1)
            return false;
        else if (!Cells[mazeCell.X + 1, mazeCell.Y].WallLeft)
            return true;
        else
            return false;
    }

}
