using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MiniGame3_Player : MonoBehaviour {
    private const KeyCode STOP_MOVE = KeyCode.Space;

    public event Action<bool> BarrierHaBeenReached;

    [SerializeField] private Ease _ease = Ease.Linear;
    
    [SerializeField] private RectTransform _bulletParent;

    private MiniGame3_BulletFactory _factory;
    private MiniGame3_Config _miniGame3_Config;

    private List<Bullet> _bullets;
    private Bullet _currentBullet;
    private Tween _rotation;
    private int _currentColorIndex;
    private int _bulletCount;
    private MiniGame3 _miniGame;

    public bool RotateDirection { get; private set; } = true;

    private float _duration => _miniGame3_Config.PlayerRotationSpeed;
    private List<Color> Colors => _miniGame3_Config.Colors;
    private bool IsGameOver => _miniGame.IsGameOver;
    private bool IsSuccess => _miniGame.IsSuccess;


    private float RotationAngle {
        get {
            if (RotateDirection)
                return 360;
            else
                return -360;
        }
    }
    private Color CurrentColor {
        get {
            _currentColorIndex = GetColorIndex();

            if (_currentColorIndex > 0 && _currentColorIndex <= Colors.Count)
                return Colors[_currentColorIndex];
            else
                return Color.clear;
        }
    }

    [Inject]
    public void Construct(MiniGame3_BulletFactory factory, MiniGame3_Config miniGame3_Config) {
        _factory = factory;
        _miniGame3_Config = miniGame3_Config;
    }

    public void Init(MiniGame3 miniGame, int bulletCount) {
        _bulletCount = bulletCount;

        _miniGame = miniGame;
        _miniGame.Started += Activate;
        _miniGame.Finished += StopRotate;

        PlayerInit();
    }

    private void Update() {

        if (Input.GetKeyDown(STOP_MOVE) && !IsGameOver && !IsSuccess) {
            LaunchBullet();
        }
    }

    public void Activate() {
        SwitchBullet();
        StartRotate();
    }

    private void PlayerInit() {
        CreateNewBullets();
    }

    private void CreateNewBullets() {
        _bullets = new List<Bullet>();

        for (int i = 0; i < _bulletCount; i++) {
            Bullet bullet = CreateNewBullet();
            bullet.BarrierTriggered += OnBarrierTriggered;

            _bullets.Add(bullet);
        }
    }

    private Bullet CreateNewBullet() {
        _currentColorIndex = GetColorIndex();
        Bullet bullet = _factory.Get(_bulletParent, _currentColorIndex);
        
        return bullet;
    }

    private int GetColorIndex() {
        if (_currentBullet == null)
            return 0;
        else {
            for (int i = 0; i < Colors.Count; i++) {
                Color color = Colors[i];

                if (color == _currentBullet.Color)
                    return i;
            }

            throw new ArgumentOutOfRangeException($"MiniGame3_Player: Invalid ColorIndex!");
        }
    }

    private void SwitchBullet() {
        if (_currentBullet != null) {
            _bullets.Remove(_currentBullet);
            Destroy(_currentBullet.gameObject);
        }

        _currentBullet = _bullets[0];
        _currentBullet.Show(true);
    }

    private void LaunchBullet() {
        _currentBullet.StartMove();
        StopRotate();
    }

    #region Rotation
    private void StartRotate() {
        _rotation = transform.DORotate(transform.eulerAngles + Vector3.forward * RotationAngle, _duration, RotateMode.FastBeyond360)
                             .SetEase(_ease)
                             .OnComplete(() => ChangeDirection());
    }

    private void StopRotate(bool status = true) {
        if (_rotation.active != false && _rotation.IsPlaying())
            _rotation.Kill();

        //transform.eulerAngles = Vector3.zero;
    }

    private void ChangeDirection() {
        _rotation.Kill();
        RotateDirection = !RotateDirection;

        StartRotate();
    }
    #endregion

    private void OnBarrierTriggered(Color color) {
        if (CurrentColor != color) {
            BarrierHaBeenReached?.Invoke(false);
            return;
        }

        BarrierHaBeenReached?.Invoke(true);
    }

    public void Dispose() {
        _miniGame.Started -= Activate;
        _miniGame.Finished -= StopRotate;
    }
}
