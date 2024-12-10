using System;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {
    public const float InitDelay = 0.1f;

    [SerializeField] private ChestConfigs _chestConfigs;
    [SerializeField] private LevelConfigs _levelConfigs;
    [SerializeField] private LevelCompanents _levelCompanents;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private MiniGamePrefabs _gamePrefabs;
    [SerializeField] private MiniGameConfigs _gameConfigs;
    [SerializeField] private MiniGame3_Config _game3Config;
    [SerializeField] private MiniGame4_Config _game4Config;

    private Logger _logger;
    
    public override void InstallBindings() {
        BindConfigs();
        BindServices();
        BindInput();

        BindUIPrefabs();
        BindFactories();

        _logger.Log("Global Installing Complited");
    }

    #region Configs
    
    private void BindConfigs() {
        BindChestConfigs();
        BindLevelConfigs();
        BindLevelCompanents();
        BindMiniGamePrefabs();
        BindMiniGameConfigs();
        BindMiniGame3Config();
        BindMiniGame4Config();
    }


    private void BindChestConfigs() {
        if (_chestConfigs != null) {
            Container.BindInstance(_chestConfigs).AsSingle();
            return;
        }

        _logger.Log($"ChestConfigs not found");
    }

    private void BindLevelConfigs() {
        if (_levelConfigs != null) {
            Container.BindInstance(_levelConfigs).AsSingle();
            return;
        }

        _logger.Log($"LevelConfigs not found");
    }

    private void BindLevelCompanents() {
        if (_levelCompanents != null) {
            Container.BindInstance(_levelCompanents).AsSingle();
            return;
        }

        _logger.Log($"LevelCompanents not found");
    }

    private void BindMiniGamePrefabs() {
        if (_gamePrefabs != null) {
            Container.BindInstance(_gamePrefabs).AsSingle();
            return;
        }

        _logger.Log($"MiniGamePrefabs not found");
    }

    private void BindMiniGameConfigs() {
        if (_gameConfigs != null) {
            Container.BindInstance(_gameConfigs).AsSingle();
            return;
        }

        _logger.Log($"MiniGameConfigs not found");
    }

    private void BindMiniGame3Config() {
        if (_game3Config != null) {
            Container.BindInstance(_game3Config).AsSingle();
            return;
        }

        _logger.Log($"MiniGame3Config not found");
    }

    private void BindMiniGame4Config() {
        if (_game4Config != null) {
            Container.BindInstance(_game4Config).AsSingle();
            return;
        }

        _logger.Log($"MiniGame4Config not found");
    }

    #endregion

    private void BindServices() {
        _logger = new Logger();
        Container.Bind<Logger>().FromInstance(_logger).AsSingle();
    }

    private void BindUIPrefabs() {
        //if (_dialogPrefabs.Prefabs.Count == 0)
        //    _logger.Log($"List of DialogPrefabs is empty");

        //Container.Bind<DialogPrefabs>().FromInstance(_dialogPrefabs).AsSingle();

        //if (_uiCompanentPrefabs.Prefabs.Count == 0)
        //    _logger.Log($"List of UICompanentPrefabs is empty");

        //Container.Bind<UICompanentPrefabs>().FromInstance(_uiCompanentPrefabs).AsSingle();
    }

    private void BindFactories() {
        //Container.Bind<DialogFactory>().AsSingle();
        //Container.Bind<UICompanentsFactory>().AsSingle();
        Container.Bind<ChestFactory>().AsSingle();

        Container.Bind<Cell>().FromInstance(_cellPrefab).AsSingle(); 
        Container.Bind<CellFactory>().AsSingle();
        Container.Bind<MiniGamesFactory>().AsSingle();
        Container.Bind<MiniGame3_BulletFactory>().AsSingle();
        Container.Bind<MainaGame4_TargetFactory>().AsSingle();

    }

    private void BindInput() {
        Container.BindInterfacesAndSelfTo<NewMoveInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<OldMoveInput>().AsSingle();

        //Container.Bind<MoveInputHandler>().AsSingle().NonLazy();
    }
}