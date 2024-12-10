using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MiniGame3_BulletFactory {
    private DiContainer _diContainer;
    private MiniGame3_Config _config;
    private Color _currentColor;

    private List<Color> _colors => _config.Colors;
    private Bullet _prefab => _config.Prefab;
    
    [Inject]
    public void Construct(DiContainer diContainer, MiniGame3_Config gameConfig) {
        _diContainer = diContainer;
        _config = gameConfig;
    }

    public Bullet Get(Transform parent, int currentColorIndex = 0) {
        _currentColor = _colors[currentColorIndex];

        Bullet newBullet = _diContainer.InstantiatePrefabForComponent<Bullet>(_prefab, parent);

        Color newColor = GetColor();
        newBullet.Init(newColor);

        return newBullet;
    }

    private Color GetColor() {
        Color newColor = _colors[Random.Range(0, _colors.Count)];

        if (newColor != _currentColor)
            return newColor;
        else
            return GetColor();
    }
}
