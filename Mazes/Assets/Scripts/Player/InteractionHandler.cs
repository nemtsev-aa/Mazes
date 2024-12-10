using UnityEngine;
using System;
using DG.Tweening;
using System.Collections;

public class InteractionHandler : MonoBehaviour, IDisposable {
    public event Action ExitReached;
    public event Action KeyCollected;
    public event Action CoinsCollected;

    public event Action<MiniGameParameters> ChestOpenMiniGameStarted;
    
    private Player _player;
    private PlayerMoveController _moveController;
    private PlayerView _view;
    private MiniGameSpawner _miniGameMediator;

    private Chest _currentChest;
    private Door _door;
    private Key _currentKey;
    private Bomb _currentBomb;

    private GameplayElement _currentElement;
    private Coins _currentCoins;

    public void Init(Player player, MiniGameSpawner miniGameMediator) {
        _player = player;
        _moveController = player.MoveController;
        _view = player.View;
        _miniGameMediator = miniGameMediator;

        _moveController.GameplayElementCollected += OnGameplayElementCollected;
        _miniGameMediator.ChestOpenMiniGameFinished += OnChestOpenMiniGameFinished;
    }


    private void OnGameplayElementCollected(GameplayElement element) {
        if (_currentElement != null)
            return;

        _currentElement = element;

        if (element is Chest) {
            _currentChest = (Chest)element;
            StartChestOpenMiniGame();
        }

        if (element is Door) {
            _door = (Door)element;
            DoorTriggered();
        }

        if (element is Key) {
            _currentKey = (Key)element;
            Collect(_currentKey);
            KeyCollected?.Invoke();
        }

        if (element is Coins) {
            _currentCoins = (Coins)element;
            Collect(_currentCoins);
            CoinsCollected?.Invoke();
        }

        if (element is Bomb) {
            _currentBomb = (Bomb)element;
            BombActivate();
        }
    }

    private void StartChestOpenMiniGame() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetDelay(2f);

        ChestOpenMiniGameStarted?.Invoke(_currentChest.Config.GameParameters);
    }

    private void DoorTriggered() {
        if (_player.HasKey == false) {
            //-// Cообщение о необходимости поиска ключа

            Debug.Log($"Key not found!");
            return;
        }

        _door.Open();

        Sequence mySequence = DOTween.Sequence();
        mySequence.SetDelay(2f);

        _moveController.MoveToDoor(_door.transform);

        ExitReached?.Invoke();

        _currentElement = null;
    }

    private void Collect(ICollected item) {
        item.MoveToTarget(_moveController.transform, DestroyCurrentElement);
    }

    private void BombActivate() {
        _currentBomb.Activate(_moveController.transform, DestroyCurrentElement);
    }

    private void OnChestOpenMiniGameFinished(bool status) {
        if (status) {
            _currentChest.Open();
            return;
        }

        _currentChest.Disappear(DestroyCurrentElement);
    }

    private void DestroyCurrentElement() {
        Destroy(_currentElement.gameObject);
        _currentElement = null;
    }

    public void Dispose() {
        _moveController.GameplayElementCollected -= OnGameplayElementCollected;
        _miniGameMediator.ChestOpenMiniGameFinished -= OnChestOpenMiniGameFinished;
    }
}

