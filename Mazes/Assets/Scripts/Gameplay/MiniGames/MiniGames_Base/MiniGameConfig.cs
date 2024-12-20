﻿using UnityEngine;
using System;

[Serializable]
public class MiniGameConfig {
    [field: SerializeField] public MiniGameTypes Type { get; private set; }
    [field: SerializeField] public MiniGameDifficultyModes DifficultyMode { get; private set; }
}
