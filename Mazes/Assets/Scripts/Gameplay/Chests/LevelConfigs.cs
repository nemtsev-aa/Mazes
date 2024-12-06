using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelConfigs), menuName = "Config/" + nameof(LevelConfigs))]
public class LevelConfigs : ScriptableObject {
    [field: SerializeField] public List<LevelConfig> Levels { get; private set; }
}
