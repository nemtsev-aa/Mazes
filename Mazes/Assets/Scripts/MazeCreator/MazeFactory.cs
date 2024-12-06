using System.Collections.Generic;
using UnityEngine;

public class MazeFactory {
    public MazeFactory(int width, int height) {
        Width = width;
        Height = height;
    }

    public MazeFactory() { }

    public int Width { get; private set; } = 50;
    public int Height { get; private set; } = 50;

    public Maze Get() {
        MazeCell[,] cells = new MazeCell[Width, Height];

        for (int x = 0; x < cells.GetLength(0); x++) {
            for (int y = 0; y < cells.GetLength(1); y++) {
                cells[x, y] = new MazeCell { X = x, Y = y };
            }
        }

        for (int x = 0; x < cells.GetLength(0); x++) {
            cells[x, Height - 1].WallLeft = false;
        }

        for (int y = 0; y < cells.GetLength(1); y++) {
            cells[Width - 1, y].WallBottom = false;
        }

        RemoveWallsWithBacktracker(cells);

        Maze maze = new Maze(cells, PlaceMazeExit(cells));

        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeCell[,] maze) {
        MazeCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeCell> stack = new Stack<MazeCell>();
        do {
            List<MazeCell> unvisitedNeighbours = new List<MazeCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited)
                unvisitedNeighbours.Add(maze[x - 1, y]);

            if (y > 0 && !maze[x, y - 1].Visited) 
                unvisitedNeighbours.Add(maze[x, y - 1]);

            if (x < Width - 2 && !maze[x + 1, y].Visited)
                unvisitedNeighbours.Add(maze[x + 1, y]);

            if (y < Height - 2 && !maze[x, y + 1].Visited)
                unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0) {
                MazeCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);

                chosen.Visited = true;
                stack.Push(chosen);
                chosen.DistanceFromStart = current.DistanceFromStart + 1;
                current = chosen;
            }
            else 
                current = stack.Pop();
            
        } while (stack.Count > 0);
    }

    private void RemoveWall(MazeCell a, MazeCell b) {
        if (a.X == b.X) {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }

    private Vector2Int PlaceMazeExit(MazeCell[,] maze) {
        MazeCell furthest = maze[0, 0];

        for (int x = 0; x < maze.GetLength(0); x++) {
            if (maze[x, Height - 2].DistanceFromStart > furthest.DistanceFromStart)
                furthest = maze[x, Height - 2];

            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart)
                furthest = maze[x, 0];
        }

        for (int y = 0; y < maze.GetLength(1); y++) {
            if (maze[Width - 2, y].DistanceFromStart > furthest.DistanceFromStart)
                furthest = maze[Width - 2, y];

            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart)
                furthest = maze[0, y];
        }

        if (furthest.X == 0)
            furthest.WallLeft = false;
        else if (furthest.Y == 0)
            furthest.WallBottom = false;
        else if (furthest.X == Width - 2)
            maze[furthest.X + 1, furthest.Y].WallLeft = false;
        else if (furthest.Y == Height - 2)
            maze[furthest.X, furthest.Y + 1].WallBottom = false;

        return new Vector2Int(furthest.X, furthest.Y);
    }
}