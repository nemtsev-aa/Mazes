using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Zenject;

public class MiniGamesFactory {
    private DiContainer _diContainer;
    private MiniGamePrefabs _gamePrefabs;
    private MiniGameConfigs _gameConfigs;

    public List<MiniGame> Games { get; private set; }
    private MiniGame _currentGame;


    [Inject]
    public void Construct(DiContainer diContainer, MiniGamePrefabs gamePrefabs, MiniGameConfigs gameConfigs) {
        _diContainer = diContainer;
        _gamePrefabs = gamePrefabs;
        _gameConfigs = gameConfigs;
    }
                                                   
    public void Init() {
        if (_gamePrefabs.Configs.Count == 0)
            throw new ArgumentNullException($"List of GamePrefabs is empty!");

        if (_gameConfigs.Configs.Count == 0)
            throw new ArgumentNullException($"List of GameConfigs is empty!");

        Games = new List<MiniGame>();
    }

    public MiniGame Get(MiniGameParameters parameters, RectTransform parent) {
        
        if (FindCreatedMiniGame(parameters))
            return _currentGame;

        return CreateNewMiniGame(parameters, parent);
    }

    private bool FindCreatedMiniGame(MiniGameParameters parameters) {
        MiniGame game = Games.FirstOrDefault(c => c.Config.Type == parameters.Type && c.Config.DifficultyMode == parameters.DifficultyMode);

        if (game == null) 
            return false;

        _currentGame = game;
        return true;
    }

    private MiniGame CreateNewMiniGame(MiniGameParameters parameters, RectTransform parent) {
        MiniGameConfig config = GetConfig(parameters);
        MiniGame prefab = GetPrefab(parameters.Type);

        _currentGame = _diContainer.InstantiatePrefabForComponent<MiniGame>(prefab, parent);
        _currentGame.Init(config);

        return _currentGame;
    }

    private MiniGameConfig GetConfig(MiniGameParameters parameters) {
        return _gameConfigs.Configs.FirstOrDefault(c => c.Type == parameters.Type && c.DifficultyMode == parameters.DifficultyMode);
    }

    private MiniGame GetPrefab (MiniGameTypes type) {
        return _gamePrefabs.Configs.FirstOrDefault(c => c.Type == type).Game;
    }
}

