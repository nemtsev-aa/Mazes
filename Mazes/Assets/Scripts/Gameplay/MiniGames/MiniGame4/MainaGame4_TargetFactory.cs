using UnityEngine;
using Zenject;

public class MainaGame4_TargetFactory {
    private DiContainer _diContainer;
    private MiniGame4_Config _config;

    private Target _prefab => _config.Prefab;

    [Inject]
    public void Construct(DiContainer diContainer, MiniGame4_Config gameConfig) {
        _diContainer = diContainer;
        _config = gameConfig;
    }

    public Target Get(Vector3 position, Transform parent) {
        return _diContainer.InstantiatePrefabForComponent<Target>(_prefab, position, Quaternion.identity, parent);
    }
}
