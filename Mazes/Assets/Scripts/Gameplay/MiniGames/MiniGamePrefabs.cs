using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(MiniGamePrefabs), menuName = "Config/" + nameof(MiniGamePrefabs))]
public class MiniGamePrefabs : ScriptableObject {
    [field: SerializeField] public List<MiniGamePrefab> Configs { get; private set; }
}


