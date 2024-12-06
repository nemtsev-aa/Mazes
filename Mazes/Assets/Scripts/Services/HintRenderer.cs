using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HintRenderer : MonoBehaviour, IDisposable {
    public event Action<float> ResetTimeChanged;

    private MazeSpawner _mazeSpawner;
    private Player _player;
    private LineRenderer _lineRenderer;
    private float _currentTime;
    private float _currentReset;

    [field: SerializeField] public float ShowDuration { get; private set; } = 5f;
    [field: SerializeField] public float ResetDelay { get; private set; } = 20f;

    public bool IsShowed { get; private set; } = false;
    public bool IsReset { get; private set; } = false;

    public MazeSpawner MazeSpawner => _mazeSpawner;
    private PlayerMoveController _playerMoveController => _player.MoveController;
    private PlayerUI _playerUI => _player.UI;
    private Maze Maze => _mazeSpawner.Maze;

    public void Init(MazeSpawner mazeSpawner, Player player) {
        _mazeSpawner = mazeSpawner;
        _player = player;

        _lineRenderer ??= GetComponent<LineRenderer>();

        _mazeSpawner.MazeHasBeenCreated += OnMazeHasBeenCreated;
        _playerUI.HintShowClicked += OnHintShowClicked;
    }

    public void DrawPath(Vector2Int startPosition, Vector2Int targetPosition) {
        if (_lineRenderer.positionCount > 0)
            _lineRenderer.positionCount = 0;

        int startX = startPosition.x;
        int startY = startPosition.y;

        int targetX = targetPosition.x;
        int targetY = targetPosition.y;

        List<Vector3> positions = new List<Vector3>();

        while ((targetX != startX || targetY != startY) && positions.Count < 10000) {
            positions.Add(new Vector3(targetX * _mazeSpawner.CellSize.x, targetY * _mazeSpawner.CellSize.y, targetY * _mazeSpawner.CellSize.z));

            MazeCell currentCell = Maze.Cells[targetX, targetY];

            if (targetX > startX && !currentCell.WallLeft && Maze.Cells[targetX - 1, targetY].DistanceFromStart < currentCell.DistanceFromStart) 
                targetX--;
            else if (targetY > startY && !currentCell.WallBottom && Maze.Cells[targetX, targetY - 1].DistanceFromStart < currentCell.DistanceFromStart)
                targetY--;
            else if (targetX < Maze.Cells.GetLength(0) - 1 && !Maze.Cells[targetX + 1, targetY].WallLeft && Maze.Cells[targetX + 1, targetY].DistanceFromStart < currentCell.DistanceFromStart)
                targetX++;
            else if (targetY < Maze.Cells.GetLength(1) - 1 && !Maze.Cells[targetX, targetY + 1].WallBottom && Maze.Cells[targetX, targetY + 1].DistanceFromStart < currentCell.DistanceFromStart)
                targetY++;
        }

        positions.Add(Vector3.zero);

        _lineRenderer.positionCount = positions.Count;
        _lineRenderer.SetPositions(positions.ToArray());
    }

    private void Update() {
        if (IsShowed) {
            _currentTime -= Time.unscaledDeltaTime;

            if (_currentTime <= 0) {
                _currentTime = 0;

                IsShowed = false;
                IsReset = true;
                _currentReset = ResetDelay;             
            }

            _lineRenderer.enabled = IsShowed;
        }    

        if (IsReset) {
            IsShowed = false;
            _currentReset -= Time.unscaledDeltaTime;

            if (_currentReset <= 0) {
                _currentReset = 0;
                IsReset = false;
            }

            ResetTimeChanged?.Invoke(_currentReset);
        }
    }

    private void OnMazeHasBeenCreated() {
        //DrawPathFromExit();
    }

    private void OnHintShowClicked() {
        if (IsShowed == false && IsReset == false) {
            _currentTime = ShowDuration;
            IsShowed = true;
        }
    }

    public void Dispose() {
        if (_mazeSpawner != null) {
            _mazeSpawner.MazeHasBeenCreated -= OnMazeHasBeenCreated;
            _mazeSpawner = null;
        }

        if (_playerUI != null)
            _playerUI.HintShowClicked -= OnHintShowClicked; 
    }
}
