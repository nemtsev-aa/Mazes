using System;
using UnityEngine;

[Serializable]
public class ChestConfig {
    [field: SerializeField] public Chest Prefab { get; private set; }
    [field: SerializeField] public ChestContentConfig ContentConfig { get; private set; }
    [field: SerializeField] public MiniGameParameters GameParameters { get; private set; }
}
