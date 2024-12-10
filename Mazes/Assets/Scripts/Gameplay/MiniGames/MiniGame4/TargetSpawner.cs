using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TargetSpawner : MonoBehaviour, IDisposable {
    public event Action<Target> CurrentTargetChanged;

    [SerializeField] private RectTransform _spawnPointParent;
    private MiniGame4 _miniGame4;
   
    private List<RectTransform> _spawnPointList;
    private RectTransform _currentSpawnPoint;

    private MainaGame4_TargetFactory _targetFactory;

    private Target _currentTarget;

    [Inject]
    public void Construct(MainaGame4_TargetFactory targetFactory) {
        _targetFactory = targetFactory;
    }

    public void Init(MiniGame4 miniGame4) {
        _miniGame4 = miniGame4;
        _miniGame4.Started += OnMiniGameStarted;


        CreateSpawnPointList();
    }

    private void OnMiniGameStarted() {
        CreateTarget();
    }

    private void CreateSpawnPointList() {
        if (_spawnPointParent.childCount == 0)
            throw new ArgumentNullException($"SpawnPoints not found");

        _spawnPointList = new List<RectTransform>();
        foreach (RectTransform iPoint in _spawnPointParent) {
            _spawnPointList.Add(iPoint);
        }

        if (_spawnPointList.Count == 0)
            throw new ArgumentNullException($"SpawnPointList is empty");
    }

    private void CreateTarget() {
        SwitchPoint();

        Target newTarget = _targetFactory.Get(_currentSpawnPoint.position, transform);
        newTarget.Init();
        newTarget.TargetShowed += CurrentTargetShowed;

        _currentTarget = newTarget;
    }

    private void SwitchPoint() {
        if (_spawnPointList.Count == 0)
            throw new ArgumentNullException($"SpawnPointList empty");

        List<RectTransform> otherSpawnPoints = new List<RectTransform>();
        otherSpawnPoints.AddRange(_spawnPointList);
        otherSpawnPoints.Remove(_currentSpawnPoint);

        int randomIndex = UnityEngine.Random.Range(0, otherSpawnPoints.Count);
        RectTransform newSpawnPoint = otherSpawnPoints[randomIndex];
        _currentSpawnPoint = newSpawnPoint;
    }

    private void CurrentTargetShowed(bool status) {
        if (status) {
            CurrentTargetChanged?.Invoke(_currentTarget);
            return;
        }

        _currentTarget.TargetShowed -= CurrentTargetShowed;
        Destroy(_currentTarget.gameObject);
        _currentTarget = null;
    }

    public void Dispose() {
        _miniGame4.Started -= OnMiniGameStarted;
    }
}
