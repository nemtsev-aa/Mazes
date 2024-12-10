using System;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {
    public event Action<bool> RotationStatusChanged;
    public event Action RotationDirectionChanged;

    [SerializeField] private RectTransform _obstaclesParent;

    private List<Obstacle> _obstacleList;
    private bool _currentDuration = true;
    private MiniGame4 _miniGame;

    private float _obstaclesRotationSpeed => _miniGame.Game_Config.ObstaclesRotationSpeed;

    public void Init(MiniGame4 miniGame) {
        _miniGame = miniGame;

        _miniGame.Started += StartRotate;
        _miniGame.Finished += StopRotate;

        InitObstacles();
    }

    private void InitObstacles() {
        if (_obstaclesParent.childCount == 0)
            throw new ArgumentNullException($"Obstacles not found");

        _obstacleList = new List<Obstacle>();
        foreach (RectTransform iRect in _obstaclesParent) {
            Obstacle obstacle = iRect.GetComponent<Obstacle>();
            _obstacleList.Add(obstacle);
        }

        if (_obstacleList.Count == 0)
            throw new ArgumentNullException($"ObstacleList is empty");


        for (int i = 0; i < _obstacleList.Count; i++) {
            _currentDuration = !_currentDuration;
            ObstacleRotationConfig config = new ObstacleRotationConfig(_currentDuration, _obstaclesRotationSpeed);
            _obstacleList[i].Init(this, config);
        }
    }

    private void StartRotate() {
        RotationStatusChanged?.Invoke(true);
    }

    private void StopRotate(bool status) {
        if (status)
            RotationStatusChanged?.Invoke(false);
        else
            ChangeDirection();
    }

    private void ChangeDirection() {
        RotationDirectionChanged?.Invoke();
    }

}
