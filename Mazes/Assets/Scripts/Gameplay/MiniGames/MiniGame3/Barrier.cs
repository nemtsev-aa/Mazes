using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Barrier : MonoBehaviour {
    [SerializeField] private Image _sprite;

    public Color Color { get; private set; }

    public void Init(Color color) {
        Color = color;
        _sprite.color = Color;
    }
}
