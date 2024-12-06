using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameplayElementsConfig), menuName = "Config/" + nameof(GameplayElementsConfig))]
public class GameplayElementsConfig : ScriptableObject {
    [field: SerializeField] public List<GameplayElementConfig> ElementConfigs = new List<GameplayElementConfig>();
}
