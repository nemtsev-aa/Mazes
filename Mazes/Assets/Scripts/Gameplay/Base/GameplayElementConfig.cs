using UnityEngine;
using System;

[Serializable]
public class GameplayElementConfig {
    [field: SerializeField] public GameplayElementTypes Type { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Sprite Background { get; private set; }
   
}
