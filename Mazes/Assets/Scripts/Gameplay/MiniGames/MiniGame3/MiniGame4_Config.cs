using UnityEngine;

[CreateAssetMenu(fileName = nameof(MiniGame4_Config), menuName = "Config/" + nameof(MiniGame4_Config))]
public class MiniGame4_Config : ScriptableObject {
    [field: SerializeField] public Target Prefab { get; private set; }
    [field: SerializeField] public float ObstaclesRotationSpeed { get; private set; }
    
}

