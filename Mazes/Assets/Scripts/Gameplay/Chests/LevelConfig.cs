using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfig {
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public int MazeSize { get; private set; } = 8;
    [field: SerializeField] public int CellSize { get; private set; } = 10;
    [field: SerializeField] public List<ChestContentTypes> ChestContents { get; private set; }
}
