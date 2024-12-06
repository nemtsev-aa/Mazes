using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ChestFactory {
    private DiContainer _container;
    private List<ChestConfig> _configs = new List<ChestConfig>();

    public ChestFactory(DiContainer container, ChestConfigs chestConfigs) {
        _container = container;
        _configs.AddRange(chestConfigs.Configs);
    }

    public Chest GetByContentType(ChestContentTypes type, Transform parent) {
        ChestConfig config = GetConfigByType(type);
        Chest newChest = _container.InstantiatePrefabForComponent<Chest>(config.Prefab, parent);
        newChest.Init(config);
        
        return newChest;
    }

    private ChestConfig GetConfigByType(ChestContentTypes type) {
        return _configs.FirstOrDefault(c => c.ContentConfig.Type == type);
    }
}
