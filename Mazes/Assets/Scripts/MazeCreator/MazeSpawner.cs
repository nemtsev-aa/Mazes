using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MazeSpawner : MonoBehaviour {
    public event Action MazeHasBeenCreated;
    public event Action GameplayElementListChanged;

    [SerializeField] private Vector2Int _mazeSize = Vector2Int.one * 10;
    [SerializeField] private int _chestCount = 3;

    private CellFactory _cellFactory;
    private ChestFactory _chestFactory;
    private LevelCompanents _levelCompanents;
    private List<Cell> _cells = new List<Cell>();
    private List<GameplayElement> _gameplayElements = new List<GameplayElement>();
    private LevelConfig _levelConfig;

    [field: SerializeField] public Vector3 CellSize { get; private set; }
    public Maze Maze { get; private set; }
    public IReadOnlyList<GameplayElement> GameplayElements => _gameplayElements;

    [Inject]
    public void Construct(CellFactory cellFactory, ChestFactory chestFactory, LevelCompanents levelCompanents) {
        _cellFactory = cellFactory;
        _chestFactory = chestFactory;
        _levelCompanents = levelCompanents;
    }

    public void Init(LevelConfig levelConfig) {
        _levelConfig = levelConfig;

        _mazeSize = new Vector2Int(_levelConfig.MazeSize, _levelConfig.MazeSize);
        _chestCount = _levelConfig.ChestContents.Count;
        _cellFactory.Init(new Vector3(_levelConfig.CellSize, 0, _levelConfig.CellSize));

        Reset();
    }

    [ContextMenu(nameof(Reset))]
    public void Reset() {
        if (Maze != null)
            DestroyMaze();

        if (_gameplayElements.Count > 0) {
            foreach (var item in _gameplayElements) {
                Destroy(item.gameObject);
            }
            _gameplayElements.Clear();
        }

        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        CreateMaze();
    }



    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }
    private void CreateMaze() {
        CreateCells();
        CreateDoor();
        CreateChests();

        MazeHasBeenCreated?.Invoke();
    }

    private void CreateCells() {
        MazeFactory factory = new MazeFactory(_mazeSize.x, _mazeSize.y);
        Maze = factory.Get();

        for (int x = 0; x < Maze.Cells.GetLength(0); x++) {
            for (int y = 0; y < Maze.Cells.GetLength(1); y++) {
                Cell c = _cellFactory.Get(x, y, transform);

                c.WallLeft.SetActive(Maze.Cells[x, y].WallLeft);
                c.WallBottom.SetActive(Maze.Cells[x, y].WallBottom);

                 c.gameObject.name = $"Cell ({x}:{y})";
                c.DistanceLabel.text = $"{Maze.Cells[x, y].DistanceFromStart}";
                c.PositionLabel.text = $"{Maze.Cells[x, y].X}:{Maze.Cells[x, y].Y} ";
                c.MazeCell = Maze.Cells[x, y];

                c.Selected += OnCellSelected;
                _cells.Add(c);
            }
        }

        CreateOutline();
    }

    private void CreateOutline() {
        float outlineX = -1;
        float outlineY = -1;

        List<Cell> outlineCells = new List<Cell>();

        Cell c = _cellFactory.Get(outlineX, outlineY, transform);
        c.transform.SetParent(transform);
        c.gameObject.name = $"Cell ({outlineX}:{outlineY})";
        outlineCells.Add(c);

        for (int x = 0; x < _mazeSize.x; x++) {
            c = _cellFactory.Get(x, outlineY, transform);
            c.transform.SetParent(transform);
            c.gameObject.name = $"Cell ({x}:{outlineY})";
            outlineCells.Add(c);
        }

        for (int y = 0; y < _mazeSize.y; y++) {
            c = _cellFactory.Get(outlineX, y, transform);
            c.transform.SetParent(transform);
            c.gameObject.name = $"Cell ({outlineX}:{y})";
            outlineCells.Add(c);
        }

        foreach (var iCell in outlineCells) {
            iCell.WallLeft.SetActive(false);
            iCell.WallBottom.SetActive(false);
        }
    }

    private void OnCellSelected(Cell cell) {
        Debug.Log($"{cell}: NeighborsCount {Maze.FindNeighborsCount(cell.MazeCell)}");
    }

    private void CreateDoor() {
        float xPosition = Maze.FinishPosition.x * CellSize.x;
        float zPosition = Maze.FinishPosition.y * CellSize.z;

        float leftMazeColumnPositionX = 0;
        float rightMazeColumnPositionX = (_mazeSize.x - 2) * CellSize.x;
        float upMazeRowPositionZ = (_mazeSize.y - 2) * CellSize.z;

        Vector3 position = new Vector3(xPosition, 0, zPosition);
        Vector3 rotate = Vector3.zero;

        if (xPosition == leftMazeColumnPositionX && zPosition > 0)
            rotate = Vector3.up * 90;
        else if (xPosition == rightMazeColumnPositionX && zPosition == upMazeRowPositionZ)
            rotate = Vector3.up * -90;
        else if (xPosition > 0 && zPosition == upMazeRowPositionZ)
            rotate = Vector3.up * 180;
        else if (xPosition == rightMazeColumnPositionX && zPosition > 0)
            rotate = Vector3.up * -90;


        Door door = Instantiate(_levelCompanents.DoorPrefab, position + new Vector3((CellSize.x / 2), 0, (CellSize.z / 2)), Quaternion.identity);
        door.transform.Rotate(rotate);
        door.transform.Translate(Vector3.back * CellSize.x);

        door.transform.SetParent(transform);
        door.SetParentMazeCell(Maze.GetCellByPosition(Maze.FinishPosition.x, Maze.FinishPosition.y));

        _gameplayElements.Add(door);
    }

    private void CreateChests() {
        // Найти самое удалённое место для размещения сундука
        List<MazeCell> deadEndsList = Maze.FindDeadEnds();
        // Сортировка по убыванию
        var sortedDeadEndsList = deadEndsList.OrderByDescending(cell => cell.DistanceFromStart).ToList();
        // Удаление из списка тупиков финишной ячейки
        MazeCell finishCell = sortedDeadEndsList.FirstOrDefault(c => c.X == Maze.FinishPosition.x && c.Y == Maze.FinishPosition.y);

        if (finishCell != null)
            sortedDeadEndsList.Remove(finishCell);

        int percent = (int)Mathf.Ceil(sortedDeadEndsList.Count * 0.3f);
        int createdChestCount = Mathf.Min(_chestCount, percent);

        for (int i = 0; i < createdChestCount; i++) {
            MazeCell firstDeadEnd = sortedDeadEndsList[UnityEngine.Random.Range(0, sortedDeadEndsList.Count)];

            // Тип сундука определить из LevelConfig
            Chest chest = _chestFactory.GetByContentType(_levelConfig.ChestContents[i], transform);
            
            MoveToMaze(firstDeadEnd, chest);
            TurnToAvailableNeighbor(firstDeadEnd, chest);

            chest.Opened += OnChestOpened;

            _gameplayElements.Add(chest);

            sortedDeadEndsList.Remove(firstDeadEnd);
        }
    }

    private void MoveToMaze(MazeCell mazeCell, Chest chest) {
        float xPosition = mazeCell.X * CellSize.x;
        float zPosition = mazeCell.Y * CellSize.z;
        Vector3 position = new Vector3(xPosition, 0, zPosition) + new Vector3((CellSize.x / 2), 0, (CellSize.z / 2));

        chest.transform.position = position;
        chest.SetParentMazeCell(Maze.GetCellByPosition(mazeCell.X, mazeCell.Y));
    }

    private void TurnToAvailableNeighbor(MazeCell firstDeadEnd, Chest chest) {
        Vector3 rotate = Vector3.zero;
        AvailableNeighborPositions neighborPosition = firstDeadEnd.GetAvailableNeighbor();

        switch (neighborPosition) {
            case AvailableNeighborPositions.Left:
                rotate = Vector3.up * -90;
                break;

            case AvailableNeighborPositions.Right:
                rotate = Vector3.up * 90;
                break;

            case AvailableNeighborPositions.Bottom:
                rotate = Vector3.up * 180;
                break;

            case AvailableNeighborPositions.Top:
                rotate = Vector3.zero;
                break;

            case AvailableNeighborPositions.None:
                rotate = Vector3.zero;
                break;

            default:
                throw new ArgumentException($"Invalid AvailableNeighborPositions: {neighborPosition}");
        }

        chest.transform.Rotate(rotate);
    }

    private void OnChestOpened(GameplayElement element) {
        Chest chest = (Chest)element;
        chest.Opened -= OnChestOpened;

        _gameplayElements.Remove(element);
        Destroy(element.gameObject);

        GameplayElementListChanged?.Invoke();
    }

    private void DestroyMaze() {
        foreach (var iCell in _cells) {
            Destroy(iCell.gameObject);
        }

        Maze = null;
        _cells.Clear();
    }

}


