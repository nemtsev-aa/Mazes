using System.Linq;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private CellFactory _cellFactory;
    [SerializeField] private MazeSpawner _mazeSpawner;
    [SerializeField] private HintRenderer _hintRenderer;
    [SerializeField] private Player _player;

    [SerializeField] private bool _newInput = true;

    private LevelConfigs _levelConfigs;
    private MiniGamesFactory _miniGamesFactory;
    private NewMoveInput _newMoveInput;
    private OldMoveInput _oldMoveInput;
    private MoveInputHandler _inputHandler;

    [Inject]
    public void Construct(NewMoveInput newMoveInput, OldMoveInput oldMoveInput, LevelConfigs levelConfigs, MiniGamesFactory miniGamesFactory) {
        _newMoveInput = newMoveInput;
        _oldMoveInput = oldMoveInput;
        _levelConfigs = levelConfigs;
        _miniGamesFactory = miniGamesFactory;

    }

    private void Start() {
        _hintRenderer.Init(_mazeSpawner, _player);
        _player.Init(GetMoveInputHandler(), _hintRenderer, _miniGamesFactory);
    }

    private MoveInputHandler GetMoveInputHandler() {
        MoveInputHandler inputHandler;

        if (_newInput)
            inputHandler = new MoveInputHandler(_newMoveInput);
        else
            inputHandler = new MoveInputHandler(_oldMoveInput);

        inputHandler.SetCellSize(_mazeSpawner.CellSize.x);

        return inputHandler;
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
            _mazeSpawner.Init(_levelConfigs.Levels.FirstOrDefault(l => l.Index == 1));

        if (Input.GetKeyDown(KeyCode.Alpha2)) 
            _mazeSpawner.Init(_levelConfigs.Levels.FirstOrDefault(l => l.Index == 2));

        if (Input.GetKeyDown(KeyCode.Alpha3))
            _mazeSpawner.Init(_levelConfigs.Levels.FirstOrDefault(l => l.Index == 3));

    }
}
