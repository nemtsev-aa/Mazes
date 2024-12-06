using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ChestConfigs), menuName = "Config/" + nameof(ChestConfigs))]
public class ChestConfigs : ScriptableObject {
    [field: SerializeField] public List<ChestConfig> Configs { get; private set; }
}
