using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(MiniGameConfigs), menuName = "Config/" + nameof(MiniGameConfigs))]
public class MiniGameConfigs : ScriptableObject {
    [field: SerializeField] public List<MiniGameConfig> Configs { get; private set; }
}


