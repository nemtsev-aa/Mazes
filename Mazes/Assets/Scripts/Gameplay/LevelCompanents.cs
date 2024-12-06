using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelCompanents), menuName = "Config/" + nameof(LevelCompanents))]
public class LevelCompanents : ScriptableObject {
    [field: SerializeField] public Door DoorPrefab { get; private set; }
    [field: SerializeField] public Key KeyPrefab { get; private set; }
    [field: SerializeField] public Chest ChestPrefab { get; private set; }


}
