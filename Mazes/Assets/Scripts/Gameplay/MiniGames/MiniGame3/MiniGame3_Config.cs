using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(MiniGame3_Config), menuName = "Config/" + nameof(MiniGame3_Config))]
public class MiniGame3_Config : ScriptableObject {
    [field: SerializeField] public float PlayerRotationSpeed { get; private set; }
    [field: SerializeField] public float BarriesRotationSpeed { get; private set; }
    [field: SerializeField] public List<Color> Colors { get; private set; }
    [field: SerializeField] public Bullet Prefab { get; private set; }
}

