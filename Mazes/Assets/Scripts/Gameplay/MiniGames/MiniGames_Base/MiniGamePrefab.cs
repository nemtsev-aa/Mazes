using UnityEngine;
using System;

[Serializable]
public class MiniGamePrefab {
    [field: SerializeField] public MiniGameTypes Type { get; private set; }
    [field: SerializeField] public MiniGame Game { get; private set; }
}

