using UnityEngine;

[System.Serializable]
public class ChestContentConfig {
    [field: SerializeField] public ChestContentTypes Type { get; private set; }
    [field: SerializeField] public GameplayElement Prefab { get; private set; }
}
